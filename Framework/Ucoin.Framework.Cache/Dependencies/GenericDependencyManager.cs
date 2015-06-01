using System.Linq;
using System.Collections.Generic;
using System;

namespace Ucoin.Framework.Cache
{
    /// <summary>
    /// A generic cache dependency mechanism that utilises no specific features
    /// of any cache engine and acts as overall support of rudimentary cache dependencies
    /// in light of the fact cache engines may not support advanced queries, dependencies
    /// and events
    /// </summary>
    public class GenericDependencyManager : BaseCacheDependencyManager
    {
        public const string CacheKeyPrefix = "__DepMgr_"; // The root cache key prefix we use
        public const string CacheDependencyEntryPrefix = "DepEntry_"; // The additional prefix for master/child cache key dependency entries

        public GenericDependencyManager(ICacheProvider cache)
            : base(cache)
        {
        }

        public override void AssociateDependentKeysToParent(string parentKey, IEnumerable<string> dependentCacheKeys)
        {
            var cacheKeyForDependency = GetParentItemCacheKey(parentKey);
            var currentEntry = Cache.Get<DependencyItem[]>(cacheKeyForDependency);
            var tempList = new List<DependencyItem>();
            if (currentEntry != null && currentEntry.Length > 0)
            {
                tempList.AddRange(currentEntry);
            }
            else
            {
                RegisterParentDependencyDefinition(parentKey);
                var items = Cache.Get<DependencyItem[]>(cacheKeyForDependency);
                if (items != null)
                {
                    tempList.AddRange(items);
                }
            }

            var keysList = new List<string>(dependentCacheKeys);
            keysList.ForEach(d =>
                                                            {
                                                                if (!tempList.Any(c => c.CacheKey == d))
                                                                {
                                                                    tempList.Add(new DependencyItem { CacheKey = d });
                                                                }
                                                            });
            //Cache.InvalidateCacheItem(cacheKeyForDependency);
            //Cache.Add(cacheKeyForDependency, GetMaxAge(), tempList.ToArray());
        }

        public override IEnumerable<DependencyItem> GetDependentCacheKeysForParent(string parentKey, bool includeParentNode = false)
        {            
            var cacheKeyForDependency = GetParentItemCacheKey(parentKey);
            var keyList = Cache.Get<DependencyItem[]>(cacheKeyForDependency);
            if (keyList == null)
            {
                RegisterParentDependencyDefinition(parentKey);
                return FilterDependencyListForParentNode(Cache.Get<DependencyItem[]>(cacheKeyForDependency), includeParentNode);
            }

            return FilterDependencyListForParentNode(keyList, includeParentNode);
        }

        private DependencyItem[] FilterDependencyListForParentNode(DependencyItem[] dependencyList, bool includeParentNode)
        {
            var depList = new List<DependencyItem>();
            if (dependencyList != null)
            {
                depList.AddRange(dependencyList);
            }

            if (!includeParentNode)
            {
                var item = depList.FirstOrDefault(d => d.IsParentNode);
                if (item != null)
                {
                    depList.Remove(item);
                }
            }
            return depList.ToArray();
        }

        public override void RegisterParentDependencyDefinition(string parentKey)
        {
            var cacheKeyForParent = GetParentItemCacheKey(parentKey);
            var item = new DependencyItem { CacheKey = parentKey, IsParentNode = true };
            var depList = new DependencyItem[] { item };
            //Cache.InvalidateCacheItem(cacheKeyForParent);
            //Cache.Add(cacheKeyForParent, GetMaxAge(), depList);
        }

        public override void RemoveParentDependencyDefinition(string parentKey)
        {
            var cacheKeyForParent = GetParentItemCacheKey(parentKey);
            //Cache.InvalidateCacheItem(cacheKeyForParent);
        }

        public override string Name
        {
            get { return "Generic/Default"; }
        }

        private DateTime GetMaxAge()
        {
            // Note: Anything above 25 causes memcached to NOT store the item with an error.
            return DateTime.Now.AddYears(10);
        }

        private string GetParentItemCacheKey(string parentKey)
        {
            var cacheKeyForParent = string.Format("{0}{1}{2}", CacheKeyPrefix, CacheDependencyEntryPrefix, parentKey);
            return cacheKeyForParent;

        }
    }
}
