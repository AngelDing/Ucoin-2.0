namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class SeatsReservationCancelled : VersionedEvent
    {
        public Guid ReservationId { get; set; }

        public IEnumerable<SeatQuantity> AvailableSeatsChanged { get; set; }
    }
}