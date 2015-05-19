namespace Ucoin.Conference.Entities.MongoDb
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ucoin.Framework.MongoDb.Entities;

    /// <summary>
    /// Represents the read model for the set of individual 
    /// seats purchased in an order, which can be assigned 
    /// to attendees.
    /// </summary>
    public class OrderSeats : StringKeyMongoEntity
    {
        public OrderSeats()
        {
            this.Seats = new List<OrderSeat>();
        }

        [BsonConstructor]
        public OrderSeats(Guid assignmentsId, Guid orderId, IEnumerable<OrderSeat> seats)
        {
            this.AssignmentsId = assignmentsId;
            this.OrderId = orderId;
            this.Seats = seats.ToList();
        }

        /// <summary>
        /// Gets or sets the seat assignments AR identifier.
        /// </summary>
        public Guid AssignmentsId { get; set; }

        /// <summary>
        /// Gets or sets the order id.
        /// </summary>
        public Guid OrderId { get; set; }

        public IList<OrderSeat> Seats { get; set; }
    }
}
