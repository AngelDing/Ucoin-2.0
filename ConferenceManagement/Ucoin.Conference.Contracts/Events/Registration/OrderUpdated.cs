
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class OrderUpdated : VersionedEvent
    {
        public IEnumerable<SeatQuantity> Seats { get; set; }
    }
}
