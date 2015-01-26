using System;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Repositories
{
    public interface IRepositoryContext : IUnitOfWork, IDisposable 
    {
        void RegisterNew<T>(T entity) where T : BaseEntity;

        void RegisterModified<T>(T entity) where T : BaseEntity;

        void RegisterDeleted<T>(T entity) where T : BaseEntity;
    }
}
