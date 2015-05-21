
namespace Ucoin.Conference.Web.Models
{
    using Ucoin.Conference.Entities.MongoDb;

    public class OrderItemViewModel
    {
        public SeatTypeView SeatType { get; set; }

        public DraftOrderItem OrderItem { get; set; }

        public bool PartiallyFulfilled { get; set; }

        public int AvailableQuantityForOrder { get; set; }

        public int MaxSelectionQuantity { get; set; }
    }
}
