
using Ucoin.Framework.Messaging;
namespace Ucoin.Conference.Repositories
{
    public class EventBus : IEventBus
    {
        public void Publish(Envelope<IEvent> @event)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(System.Collections.Generic.IEnumerable<Envelope<IEvent>> events)
        {
            throw new System.NotImplementedException();
        }
    }
}
