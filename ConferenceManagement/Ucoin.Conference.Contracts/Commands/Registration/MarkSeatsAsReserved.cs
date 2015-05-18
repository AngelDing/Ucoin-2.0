
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.Messaging;

    public class MarkSeatsAsReserved : ICommand
    {
        public MarkSeatsAsReserved()
        {
            this.Id = Guid.NewGuid();
            this.Seats = new List<SeatQuantity>();
        }

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public List<SeatQuantity> Seats { get; set; }

        public DateTime Expiration { get; set; }
    }
}
