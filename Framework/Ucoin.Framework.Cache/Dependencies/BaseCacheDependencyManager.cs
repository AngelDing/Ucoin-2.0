using System.Linq;
using System.Collections.Generic;

namespace Ucoin.Framework.Cache
{
    public abstract class BaseCacheDependencyManager : ICacheDependencyManager
    {
        private ICacheProvider _cache;

        public BaseCacheDependencyManager(ICacheProvider cache)
        {
            _cache = cache;
        }

        public ICacheProvider Cache { get { return _cache; } }

        public abstract void RegisterParentDependencyDefinition(string parentKey);

        public abstract void RemoveParentDependencyDefinition(string parentKey);

        public abstract void AssociateDependentKeysToParent(string parentKey, IEnumerable<string> dependentCacheKeys);

        public abstract IEnumerable<DependencyItem> GetDependentCacheKeysForParent(string parentKey, bool includeParentNode = false);

        public abstract string Name { get; }

        public virtual void PerformActionForDependenciesAssociatedWithParent(string parentKey)
        {
            ExecuteDefaultOrSuppliedActionForParentKeyDependencies(parentKey);
        }

        public virtual void ForceActionForDependenciesAssociatedWithParent(string parentKey)
        {
             ExecuteDefaultOrSuppliedActionForParentKeyDependencies(parentKey);
        }

        protected virtual void ExecuteDefaultOrSuppliedActionForParentKeyDependencies(string parentKey)
        {
            var alreadyProcessedKeys = new List<string>();

            var itemsToAction = GetCacheKeysToActionForParentKeyDependencies(parentKey, alreadyProcessedKeys);
            var itemsToClear = new List<string>();
            itemsToAction.ForEach(item =>
            {
                itemsToClear.Add(item.CacheKey);
            });
            if (itemsToClear.Count > 0)
            {
                //_cache.InvalidateCacheItems(itemsToClear);
            }
        }

        protected virtual List<DependencyItem> GetCacheKeysToActionForParentKeyDependencies(string parentKey, List<string> alreadyProcessedKeys = null)
        {
            var cacheKeysToAction = new List<DependencyItem>();

            if (alreadyProcessedKeys == null)
            {
                alreadyProcessedKeys = new List<string>();
            }

            var items = GetDependentCacheKeysForParent(parentKey);
            var numItems = items != null ? items.Count() : 0;
            if (numItems > 0)
            {
                foreach (var item in items)
                {
                    // Dont allow recursion
                    if (item.CacheKey == parentKey)
                    {
                        continue;
                    }
                    if (alreadyProcessedKeys.Contains(item.CacheKey))
                    {
                        continue;
                    }
                    cacheKeysToAction.Add(item);
                    alreadyProcessedKeys.Add(item.CacheKey);
                    cacheKeysToAction.AddRange(GetCacheKeysToActionForParentKeyDependencies(item.CacheKey,alreadyProcessedKeys));
                }
            }
            return cacheKeysToAction;
        }
    }
}
