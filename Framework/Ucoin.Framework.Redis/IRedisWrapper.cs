
using System;
using System.Collections.Generic;
namespace Ucoin.Framework.Redis
{
    public interface IRedisWrapper
    {
        string ConnectionId { get; }

        object Get(string key);

        void Set(string key, string dataStr, TimeSpan? expiry = null);

        bool Exists(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void ClearAll();

        void KeyExpire(string key, TimeSpan expirationTimeout);

        Dictionary<string, string> HashGetAll(string key);

        void HashSet(string key, IList<KeyValuePair<string, string>> hashItems);

        void HashDelete(string key, IList<string> dataItems);
    }
}
