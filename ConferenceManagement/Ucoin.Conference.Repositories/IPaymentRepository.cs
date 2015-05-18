using System;
using Ucoin.Conference.Entities.Payments;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public interface IPaymentRepository : IEfRepository<Payment, Guid>
    {
    }
}
