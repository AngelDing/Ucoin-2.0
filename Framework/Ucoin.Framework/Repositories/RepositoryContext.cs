using System;
using System.Collections.Generic;
using System.Threading;

namespace Ucoin.Framework.Repositories
{
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        private readonly ThreadLocal<List<object>> localNewCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localModifiedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localDeletedCollection = new ThreadLocal<List<object>>(() => new List<object>());

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.localDeletedCollection.Dispose();
                this.localModifiedCollection.Dispose();
                this.localNewCollection.Dispose();
            }
        }

        public virtual void RegisterNew(object obj)
        {
            localNewCollection.Value.Add(obj);
        }

        public virtual void RegisterModified(object obj)
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

        public virtual void RegisterDeleted(object obj)
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
