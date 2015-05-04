using System;
using System.Linq;
using System.Collections.Generic;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public class OrderRepository : EfRepository<Order, Guid>, IOrderRepository
    {
        private readonly ConferenceContext db;

        public OrderRepository(IConferenceRepositoryContext context)
            : base(context)
        {
            this.db = DbContext as ConferenceContext;
        }

        public IEnumerable<Order> FindOrders(Guid conferenceId)
        {
            return this.RetryPolicy.ExecuteAction(() => db.Orders.Include("Seats.SeatInfo")
               .Where(x => x.ConferenceId == conferenceId)
               .ToList());
        }
    }
}
