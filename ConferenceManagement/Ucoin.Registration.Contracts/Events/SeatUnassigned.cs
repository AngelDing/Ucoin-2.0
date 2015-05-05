
namespace Ucoin.Registration.Contracts
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
