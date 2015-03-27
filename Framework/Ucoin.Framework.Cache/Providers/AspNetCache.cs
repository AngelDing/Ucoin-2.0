﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.Cache
{    
    public partial class AspNetCache : BaseCache, ICacheProvider
    {
        private const string REGION_NAME = "$$GroupTour$$";
		private const string FAKE_NULL = "__[NULL]__";

        public IEnumerable<KeyValuePair<string, object>> Entries
        {
            get
            {
                if (HttpRuntime.Cache == null)
                {
                    return Enumerable.Empty<KeyValuePair<string, object>>();
                }
                
                var result = 
                    from entry in HttpRuntime.Cache.Cast<DictionaryEntry>()
                    let key = entry.Key.ToString()
                    where key.StartsWith(REGION_NAME)
                    select new KeyValuePair<string, object>(
                        key.Substring(REGION_NAME.Length),
                        entry.Value);

                return result;
            }
        }

		public object Get(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return null;
            }

			var value = HttpRuntime.Cache.Get(BuildKey(key));

            if (value.Equals(FAKE_NULL))
            {
                return null;
            }

			return value;
        }

        public void Set(CacheKey cacheKey, object value, CachePolicy cachePolicy)
        {
            if (HttpRuntime.Cache == null)
            {
                return;
            }

            var key = BuildKey(cacheKey.Key);

            var absoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
            var slidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;

            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Sliding:
                    slidingExpiration = cachePolicy.SlidingExpiration;
                    break;
                case CacheExpirationType.Absolute:
                    absoluteExpiration = cachePolicy.AbsoluteExpiration.DateTime;
                    break;
                case CacheExpirationType.Duration:
                    absoluteExpiration = DateTime.Now.Add(cachePolicy.Duration);
                    break;
                default:
                    break;
            }

            CacheDependency dependencies = null;
            var tags = GetTagKeyList(cacheKey);
            if (tags.Any())
            {
                var files = Enumerable.Empty<string>().ToArray();
                dependencies = new CacheDependency(files, tags.ToArray());
            }

            HttpRuntime.Cache.Insert(key, value ?? FAKE_NULL, dependencies, absoluteExpiration, slidingExpiration);
        }

        public bool Contains(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return false;
            }
			return HttpRuntime.Cache.Get(BuildKey(key)) != null;
        }

        public void Remove(string key)
        {
            if (HttpRuntime.Cache == null)
            {
                return;
            }
			HttpRuntime.Cache.Remove(BuildKey(key));
        }
       
		public bool IsSingleton
		{
			// because Asp.NET Cache is thread-safe by itself,
			// no need to mess up with locks.
			get { return false; }
		}

        private static string BuildKey(string key)
        {
            return key.HasValue() ? REGION_NAME + key : null;
        }
    }
}
