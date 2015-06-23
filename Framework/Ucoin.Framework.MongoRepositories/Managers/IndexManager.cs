using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Ucoin.Framework.Utils;

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
            var indexList = new List<BsonDocument>();
            AsyncHelper.RunSync(() => this.Collection.Indexes.ListAsync()
                .Result.ForEachAsync(i => indexList.Add(i)));
            var exist = false;
            foreach (var index in indexList)
            {
                foreach (var e in index.Elements)
                {
                    if (e.Name == "name" && e.Value == keyName)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
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
            this.CreateIndexes(new string[] { keyName }, keyName);
        }

        public void CreateIndexes(IEnumerable<string> keyNames, string indexName)
        {
            var builder = Builders<T>.IndexKeys;
            var keys = new List<IndexKeysDefinition<T>>();
            foreach (var name in keyNames)
            {
                var key = builder.Ascending(name);
                keys.Add(key);
            }

            var keyList = builder.Combine(keys);
            var options = new CreateIndexOptions { Name = indexName };

            this.CreateIndexes(keyList, options);
        }

        public void CreateIndexes(IndexKeysDefinition<T> keys, CreateIndexOptions options = null)
        {
            this.Collection.Indexes.CreateOneAsync(keys, options);
        }

        #endregion
    }
}
