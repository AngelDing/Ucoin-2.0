namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using Ucoin.Framework.EventSourcing;

    public class SeatsReservationCommitted : VersionedEvent
    {
        public Guid ReservationId { get; set; }
    }
}