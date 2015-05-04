namespace Ucoin.Framework.Messaging.Handling
{
    public interface IEventHandlerRegistry
    {
        void Register(IEventHandler handler);
    }
}
