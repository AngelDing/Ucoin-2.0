namespace Ucoin.Conference.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Ucoin.Conference.Contracts;
    using Ucoin.Framework.Messaging.Handling;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Framework.Messaging;
    using Ucoin.Conference.Repositories;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Contracts.Commands.Registration;
    using Ucoin.Framework.EventSourcing;

    public class ConferenceViewModelGenerator :
        IEventHandler<ConferenceCreated>,
        IEventHandler<ConferenceUpdated>,
        IEventHandler<ConferencePublished>,
        IEventHandler<ConferenceUnpublished>,
        IEventHandler<SeatCreated>,
        IEventHandler<SeatUpdated>,
        IEventHandler<AvailableSeatsChanged>,
        IEventHandler<SeatsReserved>,
        IEventHandler<SeatsReservationCancelled>
    {
        private readonly ICommandBus bus;
        private readonly ConferenceMongoRepository<ConferenceView> conferenceRepository;
        private readonly ConferenceMongoRepository<SeatTypeView> seatTypeRepository;

        public ConferenceViewModelGenerator(ICommandBus bus)
        {
            this.bus = bus;
            conferenceRepository = new ConferenceMongoRepository<ConferenceView>();
            seatTypeRepository = new ConferenceMongoRepository<SeatTypeView>();
        }

        public void Handle(ConferenceCreated @event)
        {
            var dto = GetConferenceView(@event.SourceId);
            if (dto != null)
            {
                Trace.TraceWarning(
                    "Ignoring ConferenceCreated event for conference with ID {0} as it was already created.",
                    @event.SourceId);
            }
            else
            {
                var conference = new ConferenceView(
                        @event.SourceId,
                        @event.Slug,
                        @event.Name,
                        @event.Description,
                        @event.Location,
                        @event.Tagline,
                        @event.TwitterSearch,
                        @event.StartDate);
                conferenceRepository.Insert(conference);
            }
        }

        private ConferenceView GetConferenceView(Guid conferenceId)
        {
            return conferenceRepository.GetBy(p => p.ConferenceId == conferenceId).FirstOrDefault();
        }

        public void Handle(ConferenceUpdated @event)
        {
            var dto = GetConferenceView(@event.SourceId);
            if (dto != null)
            {
                dto.Code = @event.Slug;
                dto.Description = @event.Description;
                dto.Location = @event.Location;
                dto.Name = @event.Name;
                dto.StartDate = @event.StartDate;
                dto.Tagline = @event.Tagline;
                dto.TwitterSearch = @event.TwitterSearch;

                conferenceRepository.Update(dto);
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Failed to locate Conference read model for updated conference with id {0}.",
                    @event.SourceId));
            }
        }

        public void Handle(ConferencePublished @event)
        {
            HandleIsPublished(@event.SourceId, true);
        }

        public void Handle(ConferenceUnpublished @event)
        {
            HandleIsPublished(@event.SourceId, false);
        }

        public void HandleIsPublished(Guid conferenceId, bool isPublished)
        {
            var dto = GetConferenceView(conferenceId);
            if (dto != null)
            {
                dto.SetUpdate(() => dto.IsPublished, isPublished);
                conferenceRepository.Update(p => p.ConferenceId == conferenceId, dto);
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Failed to locate Conference read model for published conference with id {0}.",
                    conferenceId));
            }
        }

        public void Handle(SeatCreated @event)
        {
            var dto = GetSeatTypeView(@event.SourceId);
            if (dto != null)
            {
                Trace.TraceWarning(
                    "Ignoring SeatCreated event for seat type with ID {0} as it was already created.",
                    @event.SourceId);
            }
            else
            {
                dto = new SeatTypeView(
                    @event.SourceId,
                    @event.ConferenceId,
                    @event.Name,
                    @event.Description,
                    @event.Price,
                    @event.Quantity);

                this.bus.Send(
                    new AddSeats
                        {
                            ConferenceId = @event.ConferenceId,
                            SeatType = @event.SourceId,
                            Quantity = @event.Quantity
                        });

                seatTypeRepository.Insert(dto);
            }
        }

        private SeatTypeView GetSeatTypeView(Guid seatTypeId)
        {
            return seatTypeRepository.GetBy(p => p.SeatTypeId == seatTypeId).FirstOrDefault();
        }

        public void Handle(SeatUpdated @event)
        {
            var dto = GetSeatTypeView(@event.SourceId);
            if (dto != null)
            {
                dto.Description = @event.Description;
                dto.Name = @event.Name;
                dto.Price = @event.Price;
                // Calculate diff to drive the seat availability.
                // Is it appropriate to have this here?
                var diff = @event.Quantity - dto.Quantity;
                dto.Quantity = @event.Quantity;

                seatTypeRepository.Update(dto);

                if (diff > 0)
                {
                    this.bus.Send(
                        new AddSeats
                            {
                                ConferenceId = @event.ConferenceId,
                                SeatType = @event.SourceId,
                                Quantity = diff,
                            });
                }
                else
                {
                    this.bus.Send(
                        new RemoveSeats
                            {
                                ConferenceId = @event.ConferenceId,
                                SeatType = @event.SourceId,
                                Quantity = Math.Abs(diff),
                            });
                }
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Failed to locate Seat Type read model being updated with id {0}.",
                    @event.SourceId));
            }
        }
        public void Handle(AvailableSeatsChanged @event)
        {
            this.UpdateAvailableQuantity(@event, @event.Seats);
        }

        public void Handle(SeatsReserved @event)
        {
            this.UpdateAvailableQuantity(@event, @event.AvailableSeatsChanged);
        }

        public void Handle(SeatsReservationCancelled @event)
        {
            this.UpdateAvailableQuantity(@event, @event.AvailableSeatsChanged);
        }

        private void UpdateAvailableQuantity(IVersionedEvent @event, IEnumerable<SeatQuantity> seats)
        {
            var seatDtos = seatTypeRepository.GetBy(x => x.ConferenceId == @event.SourceId).ToList();
            if (seatDtos.Count > 0)
            {
                // This check assumes events might be received more than once, but not out of order
                var maxSeatsAvailabilityVersion = seatDtos.Max(x => x.SeatsAvailabilityVersion);
                if (maxSeatsAvailabilityVersion >= @event.Version)
                {
                    Trace.TraceWarning(
                        "Ignoring availability update message with version {1} for seat types with conference id {0}, last known version {2}.",
                        @event.SourceId,
                        @event.Version,
                        maxSeatsAvailabilityVersion);
                    return;
                }

                foreach (var seat in seats)
                {
                    var seatDto = seatDtos.FirstOrDefault(x => x.SeatTypeId == seat.SeatType);
                    if (seatDto != null)
                    {
                        seatDto.AvailableQuantity += seat.Quantity;
                        seatDto.SeatsAvailabilityVersion = @event.Version;
                    }
                    else
                    {
                        // TODO should reject the entire update?
                        Trace.TraceError(
                            "Failed to locate Seat Type read model being updated with id {0}.", seat.SeatType);
                    }
                }

                seatTypeRepository.Update(seatDtos);
            }
            else
            {
                Trace.TraceError(
                    "Failed to locate Seat Types read model for updated seat availability, with conference id {0}.",
                    @event.SourceId);
            }
        }
    }
}
