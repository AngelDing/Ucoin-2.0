
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using Ucoin.Framework.EventSourcing;
    using Ucoin.Framework.ValueObjects;

    public class SeatAssignmentUpdated : VersionedEvent
    {
        public SeatAssignmentUpdated(Guid sourceId)
        {
            this.SourceId = sourceId;
        }

        public int Position { get; set; }

        public PersonalInfo Attendee { get; set; }
    }
}
