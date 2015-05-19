namespace Ucoin.Conference.Services
{
    using System;
    using System.Linq;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Repositories;

    public class OrderViewService : IOrderViewService
    {
        public Guid? LocateOrder(string email, string accessCode)
        {
            var rep = new ConferenceMongoRepository<DraftOrder>();
            var orderProjection = rep.GetBy(o => 
                o.RegistrantEmail == email && o.AccessCode == accessCode)
                .FirstOrDefault();

            if (orderProjection != null)
            {
                return orderProjection.OrderId;
            }

            return null;
        }

        public DraftOrder FindDraftOrder(Guid orderId)
        {
            var rep = new ConferenceMongoRepository<DraftOrder>();
            return rep.GetBy(o => o.OrderId == orderId).FirstOrDefault();
        }

        public PricedOrder FindPricedOrder(Guid orderId)
        {
            var rep = new ConferenceMongoRepository<PricedOrder>();
            return rep.GetBy(o => o.OrderId == orderId).FirstOrDefault();
        }

        public OrderSeats FindOrderSeats(Guid assignmentsId)
        {
            var rep = new ConferenceMongoRepository<OrderSeats>();
            return rep.GetBy(o => o.AssignmentsId == assignmentsId).FirstOrDefault();
        }
    }
}