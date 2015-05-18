using Ucoin.Framework.EventSourcing;

namespace Ucoin.Conference.Contracts.Events.Registration
{
    /// <summary>
    /// This event was deprecated in favor of <see cref="OrderConfirmed"/>.
    /// </summary>
    public class OrderPaymentConfirmed : VersionedEvent
    {
    }
}
