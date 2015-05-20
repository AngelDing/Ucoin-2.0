using System.Linq;
using System.Collections.Generic;
using Ucoin.Conference.Entities.MongoDb;
using Ucoin.Conference.Entities.ViewModel;
using Ucoin.Conference.IRepositories;
using System;

namespace Ucoin.Conference.Repositories
{
    public class SeatTypeViewRepository : ConferenceMongoRepository<SeatTypeView>, ISeatTypeViewRepository
    {
        public IList<SeatTypeView> GetPublishedSeatTypes(Guid conferenceId)
        { 
            return this.GetBy(c => c.ConferenceId == conferenceId).ToList();           
        }
    }
}
