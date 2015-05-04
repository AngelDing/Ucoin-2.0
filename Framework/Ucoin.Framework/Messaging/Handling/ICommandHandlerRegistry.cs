
namespace Ucoin.Framework.Messaging.Handling
{
    public interface ICommandHandlerRegistry
    {
        void Register(ICommandHandler handler);
    }
}
