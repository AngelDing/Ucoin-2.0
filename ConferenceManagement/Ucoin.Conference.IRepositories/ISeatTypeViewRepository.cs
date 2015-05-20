
using System;
using System.Collections.Generic;
using Ucoin.Conference.Entities.MongoDb;
using Ucoin.Conference.Entities.ViewModel;
using Ucoin.Framework.MongoDb.Repositories;

namespace Ucoin.Conference.IRepositories
{
    public interface ISeatTypeViewRepository : IMongoRepository<SeatTypeView, string>
    {
        IList<SeatTypeView> GetPublishedSeatTypes(Guid conferenceId);
    }
}
