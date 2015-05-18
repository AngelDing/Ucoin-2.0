using System;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Entities.Payments;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public class PaymentRepository : EfRepository<Payment, Guid>, IPaymentRepository
    {
        private readonly ConferenceContext db;

        public PaymentRepository(IConferenceRepositoryContext context)
            : base(context)
        {
            this.db = DbContext as ConferenceContext;
        }
    }
}
