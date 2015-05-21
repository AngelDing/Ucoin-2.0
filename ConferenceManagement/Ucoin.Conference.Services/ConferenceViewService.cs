namespace Ucoin.Conference.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Entities.ViewModel;
    using Ucoin.Conference.Repositories;
    using Ucoin.Framework.MongoDb;

    public class ConferenceViewService : BaseMongoService, IConferenceViewService
    {
        public ConferenceDetails GetConferenceDetails(string conferenceCode)
        {
            var conferenceList = GetConferenceByCode(conferenceCode);
            return conferenceList.Select(x =>
                new ConferenceDetails
                {
                    Id = x.ConferenceId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    Tagline = x.Tagline,
                    TwitterSearch = x.TwitterSearch,
                    StartDate = x.StartDate
                })
                .FirstOrDefault();
        }

        public ConferenceAlias GetConferenceAlias(string conferenceCode)
        {
            var conferenceList = GetConferenceByCode(conferenceCode);
            return conferenceList.Select(x =>
                new ConferenceAlias
                {
                    Id = x.ConferenceId,
                    Code = x.Code,
                    Name = x.Name,
                    Tagline = x.Tagline
                })
                .FirstOrDefault();
        }

        private IEnumerable<ConferenceView> GetConferenceByCode(string conferenceCode)
        {
            var rep = new ConferenceMongoRepository<ConferenceView>();

            return rep.GetBy(dto => dto.Code == conferenceCode);
        }


        public IList<ConferenceAlias> GetPublishedConferences()
        {
            var alias = new List<ConferenceAlias>();
            var rep = new ConferenceMongoRepository<ConferenceView>();
            var conferenceList = rep.GetBy(dto => dto.IsPublished);
            if (conferenceList.Count() > 0)
            {
                alias = conferenceList.Select(x =>
                    new ConferenceAlias
                    {
                        Id = x.ConferenceId,
                        Code = x.Code,
                        Name = x.Name,
                        Tagline = x.Tagline
                    })
                    .ToList();
            }
            return alias;
        }

        public IList<SeatTypeView> GetPublishedSeatTypes(Guid conferenceId)
        {
            var rep = new SeatTypeViewRepository();
            return rep.GetPublishedSeatTypes(conferenceId);
        }

        public IList<SeatTypeName> GetSeatTypeNames(IEnumerable<Guid> seatTypes)
        {
            var distinctIds = seatTypes.Distinct().ToArray();
            if (distinctIds.Length == 0)
            {
                return new List<SeatTypeName>();
            }

            var rep = new ConferenceMongoRepository<SeatTypeView>();
            return rep.GetBy(x => distinctIds.Contains(x.SeatTypeId))
                .Select(s => new SeatTypeName { Id = s.SeatTypeId, Name = s.Name })
                .ToList();
        }
    }
}