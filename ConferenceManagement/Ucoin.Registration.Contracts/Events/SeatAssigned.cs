
namespace Ucoin.Registration.Contracts
{
    using System;
    using Ucoin.Framework.EventSourcing;

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
