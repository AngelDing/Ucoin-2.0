
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class SeatAssignmentsCreated : VersionedEvent
    {
        public class SeatAssignmentInfo
        {
            public int Position { get; set; }
            public Guid SeatType { get; set; }
        }

        public Guid OrderId { get; set; }
        public IEnumerable<SeatAssignmentInfo> Seats { get; set; }
    }
}
