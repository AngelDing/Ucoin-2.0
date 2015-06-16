using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class IndexManager<T> : BaseMongoDB<T>, IIndexManager<T> where T : class
    {
        public IndexManager(string connectionString)
            : this(connectionString, null)
        {
        }

        public IndexManager(string connectionString, string collName)
            : base(connectionString, collName)
        {
        }

        #region Index

        public bool IndexExists(string keyName)
        {
            var indexList = new List<string>();
            this.Collection.Indexes.ListAsync().Result.ForEachAsync(i => indexList.Add(i.ToString()));
            return indexList.Contains(keyName);
        }

        public void DropIndex(string keyName)
        {
            this.DropIndexes(new string[] { keyName });
        }

        public void DropIndexes(IEnumerable<string> keyNames)
        {
            foreach (var key in keyNames)
            {
                this.DropIndexByName(key);
            }
        }

        public void DropIndexByName(string indexName)
        {
            this.Collection.Indexes.DropOneAsync(indexName);
        }

        public void CreateIndex(string keyName)
        {
            this.CreateIndexes(new string[] { keyName });
        }

        public void CreateIndexes(IEnumerable<string> keyNames)
        {
             var builder = Builders<T>.IndexKeys;
            var keys = new List<IndexKeysDefinition<T>>();
            foreach(var name in keyNames)
            {
                var key = builder.Ascending(name);
                keys.Add(key);
            }

            var keyList = builder.Combine(keys);

            this.CreateIndexes(keyList);
        }

        public void CreateIndexes(IndexKeysDefinition<T> keys, CreateIndexOptions options = null)
        {
            this.Collection.Indexes.CreateOneAsync(keys, options);
        }

        #endregion
    }
}
