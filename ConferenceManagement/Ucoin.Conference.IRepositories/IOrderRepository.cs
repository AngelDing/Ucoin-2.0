using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ucoin.Conference.Entities;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Conference.Repositories
{
    public interface IOrderRepository : IEfRepository<Order, Guid>
    {
        IEnumerable<Order> FindOrders(Guid conferenceId);

        Order FindOrder(Expression<Func<Order, bool>> lookup);
    }
}
