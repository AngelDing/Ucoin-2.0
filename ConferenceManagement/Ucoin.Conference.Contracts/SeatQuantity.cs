
namespace Ucoin.Conference.Contracts
{
    using System;

    public struct SeatQuantity
    {
        private Guid seatType;
        private int quantity;

        public SeatQuantity(Guid seatType, int quantity)
        {
            this.seatType = seatType;
            this.quantity = quantity;
        }

        public Guid SeatType
        {
            get { return this.seatType; }
            set { this.seatType = value; }
        }

        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
    }
}
