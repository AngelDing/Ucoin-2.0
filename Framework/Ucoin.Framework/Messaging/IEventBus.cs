
namespace Ucoin.Framework.Messaging
{
    using System.Collections.Generic;

    /// <summary>
    /// An event bus that sends serialized object payloads.
    /// </summary>
    /// <remarks>Note that <see cref="Ucoin.Framework.EventSourcing.IEventSourced"/> entities persisted through 
    /// the <see cref="Ucoin.Framework.EventSourcing.IEventSourcedRepository{T}"/> do not
    /// use the <see cref="IEventBus"/>, but has its own event publishing mechanism.</remarks>
    public interface IEventBus
    {
        void Publish(Envelope<IEvent> @event);
        void Publish(IEnumerable<Envelope<IEvent>> events);
    }
}
