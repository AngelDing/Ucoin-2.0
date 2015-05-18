
namespace Ucoin.Conference.Contracts.Events.Registration
{
    using Ucoin.Framework.EventSourcing;

    public class OrderTotalsCalculated : VersionedEvent
    {
        public decimal Total { get; set; }

        public OrderLine[] Lines { get; set; }

        public bool IsFreeOfCharge { get; set; }
    }
}
