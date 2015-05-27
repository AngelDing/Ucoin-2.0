using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading;
using Ucoin.Framework.Threading;

namespace Ucoin.Framework.Cache
{
    public abstract class BaseCache
    {
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private const string tagKey = "global::tag::{0}";

        internal IList<string> GetTagKeyList(CacheKey key)
        {
           return  key.Tags.Select(GetTagKey).ToList();            
        }

        internal string GetTagKey(CacheTag t)
        {
            return string.Format(tagKey, t);
        }

        internal string GetKey(CacheKey cacheKey)
        {
            return cacheKey.Key;
        }

        public abstract bool IsSingleton();

        public IDisposable EnterReadLock()
        {
            return IsSingleton() ? rwLock.GetUpgradeableReadLock() : ActionDisposable.Empty;
        }

        public IDisposable EnterWriteLock()
        {
            return IsSingleton() ? rwLock.GetWriteLock() : ActionDisposable.Empty;
        }
    }
}
