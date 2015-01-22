using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ucoin.Framework.CompareObjects
{
    public class Cache
    {
        private static readonly Dictionary<Type, PropertyInfo[]> propertyCache;
        private static readonly Dictionary<Type, PropertyInfo[]> keyPropertyCache;

        static Cache()
        {
            propertyCache = new Dictionary<Type, PropertyInfo[]>();
            keyPropertyCache = new Dictionary<Type, PropertyInfo[]>();
        }
        public static void ClearCache()
        {
            lock (propertyCache)
            {
                propertyCache.Clear();
            }
            lock (keyPropertyCache)
            {
                keyPropertyCache.Clear();
            }
        }

        public static IEnumerable<PropertyInfo> GetKeyPropertyInfo(Type type)
        {
            lock (keyPropertyCache)
            {
                if (keyPropertyCache.ContainsKey(type))
                {
                    return keyPropertyCache[type];
                }

                var keyList = new List<PropertyInfo>();
                var all = GetPropertyInfo(type);
                foreach (var p in all)
                {
                    var attributes = p.GetCustomAttributes(true);
                    var keyAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(CompareKeyAttribute));
                    if (keyAttribute != null)
                    {
                        keyList.Add(p);
                    }
                }
                keyPropertyCache.Add(type, keyList.ToArray());
                return keyList;
            }
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfo(Type type)
        {
            lock (propertyCache)
            {
                if (propertyCache.ContainsKey(type))
                {
                    return propertyCache[type];
                }

                PropertyInfo[] currentProperties;

                currentProperties = type.GetProperties();

                propertyCache.Add(type, currentProperties);

                return currentProperties;
            }
        }
    }
}
