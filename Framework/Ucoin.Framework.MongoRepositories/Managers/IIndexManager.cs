using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Ucoin.Framework.MongoDb.Managers
{
    public interface IIndexManager<T>
        where T : class
    {
        #region Index

        bool IndexExists(string keyName);

        void DropIndex(string keyName);

        void DropIndexByName(string indexName);

        void DropIndexes(IEnumerable<string> keyNames);

        void CreateIndex(string keyName);

        void CreateIndexes(IEnumerable<string> keyNames, string indexName);

        void CreateIndexes(IndexKeysDefinition<T> keys, CreateIndexOptions options = null);

        #endregion
    }
}
