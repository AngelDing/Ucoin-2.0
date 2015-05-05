
namespace Ucoin.Registration.Contracts
{
    using System;
    using Ucoin.Framework.EventSourcing;

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
