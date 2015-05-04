using System;
using Ucoin.Conference.Entities;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public interface ISeatTypeRepository : IEfRepository<SeatType, Guid>
    {
    }
}
