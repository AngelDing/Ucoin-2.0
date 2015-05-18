
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System;
    using Ucoin.Framework.EventSourcing;

    public class SeatUnassigned : VersionedEvent
    {
        public SeatUnassigned(Guid sourceId)
        {
            this.SourceId = sourceId;
        }

        public int Position { get; set; }
    }
}
