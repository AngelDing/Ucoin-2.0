
using StackExchange.Redis;
using System;
using System.Collections.Generic;
namespace Ucoin.Framework.Redis
{
    public class StackExchangeRedisWrapper : IRedisWrapper
    {
        private static IDatabase db = null;
        private readonly StackExchangeRedisFactory factory;

        public StackExchangeRedisWrapper()
        {
            factory = new StackExchangeRedisFactory();
            db = factory.GetDatabase();
        }

        public object Get(string key)
        {
            var data = db.StringGet(key);

            return data;
        }

        public void Set(string key, string dataStr, TimeSpan? expiry = null)
        {
            db.StringSet(key, dataStr, expiry);
        }

        public bool Exists(string key)
        {
            return db.KeyExists(key); 
        }

        public void Remove(string key)
        {
            db.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var keys = new List<RedisKey>();

            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                var dbKeys = db.Multiplexer.GetServer(endpoint).Keys(pattern: pattern);

                foreach (var dbKey in dbKeys)
                {
                    if (!keys.Contains(dbKey))
                    {
                        keys.Add(dbKey);
                    }
                }
            }

            keys.ForEach(k => Remove(k));
        }

        public void ClearAll()
        {
            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                if (factory.IsEndPointReadonly(endpoint.ToString()) == false)
                {
                    db.Multiplexer.GetServer(endpoint).FlushDatabase(db.Database);
                }
            }
        }
    }
}
