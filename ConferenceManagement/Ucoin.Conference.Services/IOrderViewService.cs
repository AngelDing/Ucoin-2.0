namespace Ucoin.Conference.Services
{
    using System;
    using Ucoin.Conference.Entities.MongoDb;

    public interface IOrderViewService
    {
        DraftOrder FindDraftOrder(Guid orderId);

        Guid? LocateOrder(string email, string accessCode);

        PricedOrder FindPricedOrder(Guid orderId);

        OrderSeats FindOrderSeats(Guid assignmentsId);
    }
}