using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.MongoRepository.Manager
{
    public class IndexManager<T> : BaseMongoDB<BsonDocument>, IIndexManager<BsonDocument> where T : class
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
            return this.IndexesExists(new string[] { keyName });
        }

        public bool IndexesExists(IEnumerable<string> keyNames)
        {
            return this.Collection.IndexExists(keyNames.ToArray());
        }

        public void DropIndex(string keyName)
        {
            this.DropIndexes(new string[] { keyName });
        }

        public void DropIndexes(IEnumerable<string> keyNames)
        {
            this.Collection.DropIndex(keyNames.ToArray());
        }

        public void DropIndexByName(string indexName)
        {
            this.Collection.DropIndexByName(indexName);
        }

        public void CreateIndex(string keyName)
        {
            this.CreateIndexes(new string[] { keyName });
        }

        public void CreateIndexes(IEnumerable<string> keyNames)
        {
            var ixk = new IndexKeysBuilder();
            ixk.Ascending(keyNames.ToArray());
            var option = new IndexOptionsBuilder().SetUnique(false).SetSparse(false);
            this.CreateIndexes(ixk, option);
        }

        public void CreateIndexes(IMongoIndexKeys keys, IMongoIndexOptions options)
        {
            this.Collection.CreateIndex(keys, options);
        }

        public void ReIndex()
        {
            this.Collection.ReIndex();
        }

        #endregion
    }
}
