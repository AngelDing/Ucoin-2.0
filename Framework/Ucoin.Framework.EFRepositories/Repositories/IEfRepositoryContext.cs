using System.Data.Entity;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public interface IEfRepositoryContext : IRepositoryContext
    {
        DbContext DbContext { get; }
    }
}
