
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using Ucoin.Framework.EventSourcing;
    using Ucoin.Framework.ValueObjects;

    public class SeatAssigned : VersionedEvent
    {
        public SeatAssigned(Guid sourceId)
        {
            this.SourceId = sourceId;
        }

        public int Position { get; set; }
        public Guid SeatType { get; set; }
        public PersonalInfo Attendee { get; set; }
    }
}
