
namespace Ucoin.Framework.Cache
{
    public abstract class DistributedCache : BaseCache
    {
        private readonly ICacheDependencyManager dependencyManager;
        public DistributedCache(ICacheDependencyManager dependencyManager)
        {
            this.dependencyManager = dependencyManager;
        }

        internal void ManageCacheDependencies(string dataToAdd, CacheKey cacheKey)
        {
            //if (_cacheDependencyManager == null)
            //{
            //    return;
            //}
            //if (_cacheDependencyManager.IsOkToActOnDependencyKeysForParent(parentKey) && dataToAdd != null)
            //{
            //    _cacheDependencyManager.AssociateDependentKeysToParent(parentKey, new string[1] { cacheKey }, action);
            //}
        }
    }
}
