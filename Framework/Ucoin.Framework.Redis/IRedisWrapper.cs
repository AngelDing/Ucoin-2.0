
using System;
namespace Ucoin.Framework.Redis
{
    public interface IRedisWrapper
    {
        object Get(string key);

        void Set(string key, string dataStr, TimeSpan? expiry = null);

        bool Exists(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void ClearAll();
    }
}
