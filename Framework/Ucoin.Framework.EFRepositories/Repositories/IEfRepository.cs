
using System.Data.Entity;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public interface IEfRepository<T, TKey> : IRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        DbContext DbContext { get; }
    }
}
