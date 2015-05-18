
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class SeatsReserved : VersionedEvent
    {
        public Guid ReservationId { get; set; }

        public IEnumerable<SeatQuantity> ReservationDetails { get; set; }

        public IEnumerable<SeatQuantity> AvailableSeatsChanged { get; set; }
    }
}