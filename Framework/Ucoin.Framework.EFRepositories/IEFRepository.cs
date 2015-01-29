
using System.Data.Entity;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.EFRepository
{
    public interface IEFRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        DbContext DbContext { get; }
    }
}
