
namespace Ucoin.Framework.Entities
{
    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {
    }

    public interface IAggregateRoot
    {
    }
}
