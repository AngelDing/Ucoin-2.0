using System.Linq;
using System.Collections.Generic;

namespace Ucoin.Framework.Cache
{
    public class BaseCache
    {
        private const string tagKey = "global::tag::{0}";

        internal IList<string> GetTagKeyList(CacheKey key)
        {
           return  key.Tags.Select(GetTagKey).ToList();            
        }

        private string GetTagKey(CacheTag t)
        {
            return string.Format(tagKey, t);
        }

        internal string GetKey(CacheKey cacheKey)
        {
            return cacheKey.Key;
        }
    }
}
