using System;
using System.Collections.Generic;
using System.Threading;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Repositories
{
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        private readonly ThreadLocal<List<IEntity>> localNewCollection = new ThreadLocal<List<IEntity>>(() => new List<IEntity>());
        private readonly ThreadLocal<List<IEntity>> localModifiedCollection = new ThreadLocal<List<IEntity>>(() => new List<IEntity>());
        private readonly ThreadLocal<List<IEntity>> localDeletedCollection = new ThreadLocal<List<IEntity>>(() => new List<IEntity>());

        protected IEnumerable<object> NewCollection
        {
            get { return localNewCollection.Value; }
        }

        protected IEnumerable<object> ModifiedCollection
        {
            get { return localModifiedCollection.Value; }
        }

        protected IEnumerable<object> DeletedCollection
        {
            get { return localDeletedCollection.Value; }
        }

        protected void ClearRegistrations()
        {
            this.localNewCollection.Value.Clear();
            this.localModifiedCollection.Value.Clear();
            this.localDeletedCollection.Value.Clear();
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this.localDeletedCollection.Dispose();
                this.localModifiedCollection.Dispose();
                this.localNewCollection.Dispose();
            }
        }

        public virtual void RegisterNew<T>(T obj) where T : BaseEntity
        {
            localNewCollection.Value.Add(obj);
        }

        public virtual void RegisterModified<T>(T obj) where T : BaseEntity
        {
            if (localDeletedCollection.Value.Contains(obj))
            {
                throw new InvalidOperationException(
                    "The object cannot be registered as a modified object since it was marked as deleted.");
            }
            if (!localModifiedCollection.Value.Contains(obj) && !localNewCollection.Value.Contains(obj))
            {
                localModifiedCollection.Value.Add(obj);
            }
        }

        public virtual void RegisterDeleted<T>(T obj) where T : BaseEntity
        {
            if (localNewCollection.Value.Contains(obj))
            {
                if (localNewCollection.Value.Remove(obj))
                {
                    return;
                }
            }
            bool removedFromModified = localModifiedCollection.Value.Remove(obj);
            if (!localDeletedCollection.Value.Contains(obj))
            {
                localDeletedCollection.Value.Add(obj);
            }
        }

        #region IUnitOfWork

        public abstract void Commit();

        #endregion
    }
}
