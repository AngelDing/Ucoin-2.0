using System.Data.Entity;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.EFRepository
{
    public interface IEFRepositoryContext : IRepositoryContext
    {
        DbContext Context { get; }
    }
}
