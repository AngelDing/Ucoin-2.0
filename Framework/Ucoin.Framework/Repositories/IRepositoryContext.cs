using System;

namespace Ucoin.Framework.Repositories
{
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        void RegisterNew(object obj);

        void RegisterModified(object obj);

        void RegisterDeleted(object obj);
    }
}
