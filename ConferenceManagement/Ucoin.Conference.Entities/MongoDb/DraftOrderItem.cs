namespace Ucoin.Conference.Entities.MongoDb
{
    using System;

    public class DraftOrderItem
    {
        public DraftOrderItem(Guid seatType, int requestedSeats)
        {
            this.SeatType = seatType;
            this.RequestedSeats = requestedSeats;
        }

        public Guid OrderId { get; private set; }

        public Guid SeatType { get; set; }

        public int RequestedSeats { get; set; }

        public int ReservedSeats { get; set; }
    }
}
