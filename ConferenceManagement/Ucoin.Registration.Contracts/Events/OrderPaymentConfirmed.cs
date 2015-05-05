using Ucoin.Framework.EventSourcing;

namespace Ucoin.Registration.Contracts
{
    /// <summary>
    /// This event was deprecated in favor of <see cref="OrderConfirmed"/>.
    /// </summary>
    public class OrderPaymentConfirmed : VersionedEvent
    {
    }
}
