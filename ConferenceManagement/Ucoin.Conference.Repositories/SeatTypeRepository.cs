using System;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public class SeatTypeRepository : EfRepository<SeatType, Guid>, ISeatTypeRepository
    {
        public SeatTypeRepository(IConferenceRepositoryContext context)
            : base(context)
        {
        }
    }
}
