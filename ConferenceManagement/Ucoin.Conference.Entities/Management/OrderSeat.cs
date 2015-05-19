using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Conference.Entities
{
    public class OrderSeat : EfEntity<Guid>
    {
        public OrderSeat(Guid orderId, int position, Guid seatInfoId)
            : this()
        {
            this.OrderId = orderId;
            this.Position = position;
            this.SeatInfoId = seatInfoId;
        }

        protected OrderSeat()
        {
            this.Attendee = new Attendee();
        }

        public int Position { get; set; }

        public Guid OrderId { get; set; }

        public Attendee Attendee { get; set; }

        /// <summary>
        /// Typical pattern for foreign key relationship 
        /// in EF. The identifier is all that's needed 
        /// to persist the referring entity.
        /// </summary>
        [ForeignKey("SeatInfo")]
        public Guid SeatInfoId { get; set; }

        public SeatType SeatInfo { get; set; }
    }
}
