using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity.Core;
using Ucoin.Conference.Entities;
using Ucoin.Conference.EfData;
using Ucoin.Framework.Messaging;
using Ucoin.Conference.Contracts;
using Ucoin.Conference.Repositories;
using Ucoin.Framework.Service;
using Ucoin.Framework.Repositories;

namespace Ucoin.Conference.Services
{
    public class ConferenceService : BaseService, IConferenceService
    {
        private readonly IEventBus eventBus;
        private readonly IConferenceRepository conferenceRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ISeatTypeRepository seatTypeRepository;

        public ConferenceService(
            IEventBus eventBus,
            IRepositoryContext context,
            IConferenceRepository conferenceRepository,
            IOrderRepository orderRepository,
            ISeatTypeRepository seatTypeRepository)
            : base(context)
        {
            this.eventBus = eventBus;
            this.conferenceRepository = conferenceRepository;
            this.orderRepository = orderRepository;
            this.seatTypeRepository = seatTypeRepository;
        }

        public void CreateConference(ConferenceInfo conference)
        {
            var existingSlug = conferenceRepository
                .GetBy(c => c.Slug == conference.Slug)
                .Select(c => c.Slug)
                .Any();

            if (existingSlug)
            {
                throw new DuplicateNameException("The chosen conference slug is already taken.");
            }

            if (conference.IsPublished)
            {
                conference.IsPublished = false;
            }

            this.Context.RegisterNew(conference);
            this.Context.Commit();

            this.PublishConferenceEvent<ConferenceCreated>(conference);
        }

        public void CreateSeat(Guid conferenceId, SeatType seat)
        {
            var conference = conferenceRepository.GetByKey(conferenceId);
            if (conference == null)
            {
                throw new ObjectNotFoundException();
            }

            Context.RegisterNew(seat);
            Context.Commit();

            if (conference.WasEverPublished)
            {
                this.PublishSeatCreated(conferenceId, seat);
            }
        }

        public ConferenceInfo FindConference(string slug)
        {
            return conferenceRepository.GetBy(x => x.Slug == slug).FirstOrDefault();
        }

        public ConferenceInfo FindConference(string email, string accessCode)
        {
            return conferenceRepository.GetBy(x => x.OwnerEmail == email && x.AccessCode == accessCode)
                .FirstOrDefault();
        }

        public IEnumerable<SeatType> FindSeatTypes(Guid conferenceId)
        {

            return conferenceRepository.FindSeatTypes(conferenceId);
        }

        public SeatType FindSeatType(Guid seatTypeId)
        {
            return seatTypeRepository.GetByKey(seatTypeId);
        }

        public IEnumerable<Order> FindOrders(Guid conferenceId)
        {
            return this.orderRepository.FindOrders(conferenceId);
        }

        public void UpdateConference(ConferenceInfo conference)
        {
            var existing = this.conferenceRepository.GetByKey(conference.Id);
            if (existing == null)
            {
                throw new ObjectNotFoundException();
            }

            this.Context.RegisterModified(conference);

            this.Context.Commit();

            this.PublishConferenceEvent<ConferenceUpdated>(conference);
        }

        public void UpdateSeat(Guid conferenceId, SeatType seat)
        {
            var existing = this.seatTypeRepository.GetByKey(seat.Id);
            if (existing == null)
            {
                throw new ObjectNotFoundException();
            }

            this.Context.RegisterModified(seat);
            this.Context.Commit();

            var wasEverPublished = this.conferenceRepository
                .GetBy(x => x.Id == conferenceId)
                .Select(x => x.WasEverPublished)
                .FirstOrDefault();
            if (wasEverPublished)
            {
                this.eventBus.Publish(new SeatUpdated
                {
                    ConferenceId = conferenceId,
                    SourceId = seat.Id,
                    Name = seat.Name,
                    Description = seat.Description,
                    Price = seat.Price,
                    Quantity = seat.Quantity,
                });
            }
        }

        public void Publish(Guid conferenceId)
        {
            this.UpdatePublished(conferenceId, true);
        }

        public void Unpublish(Guid conferenceId)
        {
            this.UpdatePublished(conferenceId, false);
        }
               
        public void DeleteSeat(Guid id)
        {

            var seat = this.seatTypeRepository.GetByKey(id);
            if (seat == null)
            {
                throw new ObjectNotFoundException();
            }

            var wasPublished = this.conferenceRepository.GetBy(x => x.Seats.Any(s => s.Id == id))
                .Select(x => x.WasEverPublished)
                .FirstOrDefault();

            if (wasPublished)
            {
                throw new InvalidOperationException("Can't delete seats from a conference that has been published at least once.");
            }

            this.Context.RegisterDeleted(seat);
            this.Context.Commit();
        }

        #region Private Methods

        private void UpdatePublished(Guid conferenceId, bool isPublished)
        {

            var conference = this.conferenceRepository.GetByKey(conferenceId);
            if (conference == null)
            {
                throw new ObjectNotFoundException();
            }

            conference.IsPublished = isPublished;
            if (isPublished && !conference.WasEverPublished)
            {
                // This flags prevents any further seat type deletions.
                conference.WasEverPublished = true;
                //this.Context.RegisterModified(conference);
                this.Context.Commit();

                // We always publish events *after* saving to store.
                // Publish all seats that were created before.
                foreach (var seat in conference.Seats)
                {
                    PublishSeatCreated(conference.Id, seat);
                }
            }
            else
            {
                this.Context.Commit();
            }

            if (isPublished)
                this.eventBus.Publish(new ConferencePublished { SourceId = conferenceId });
            else
                this.eventBus.Publish(new ConferenceUnpublished { SourceId = conferenceId });
        }

        private void PublishConferenceEvent<T>(ConferenceInfo conference)
            where T : ConferenceEvent, new()
        {
            this.eventBus.Publish(new T()
            {
                SourceId = conference.Id,
                Owner = new Owner
                {
                    Name = conference.OwnerName,
                    Email = conference.OwnerEmail,
                },
                Name = conference.Name,
                Description = conference.Description,
                Location = conference.Location,
                Slug = conference.Slug,
                Tagline = conference.Tagline,
                TwitterSearch = conference.TwitterSearch,
                StartDate = conference.BookableDateRange.StartDateTime,
                EndDate = conference.BookableDateRange.EndDateTime,
            });
        }

        private void PublishSeatCreated(Guid conferenceId, SeatType seat)
        {
            this.eventBus.Publish(new SeatCreated
            {
                ConferenceId = conferenceId,
                SourceId = seat.Id,
                Name = seat.Name,
                Description = seat.Description,
                Price = seat.Price,
                Quantity = seat.Quantity,
            });
        }

        #endregion
    }
}
