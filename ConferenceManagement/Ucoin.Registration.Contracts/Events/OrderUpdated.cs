
namespace Ucoin.Registration.Contracts
{
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class OrderUpdated : VersionedEvent
    {
        public IEnumerable<SeatQuantity> Seats { get; set; }
    }
}
