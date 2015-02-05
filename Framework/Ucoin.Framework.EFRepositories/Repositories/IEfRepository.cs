
using System.Data.Entity;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public interface IEfRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        DbContext DbContext { get; }
    }
}
