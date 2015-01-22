using MongoDB.Driver;
using System.Collections.Generic;

namespace Ucoin.Framework.MongoRepository.Manager
{
    public interface IIndexManager<T>
        where T : class
    {
        #region Index

        bool IndexExists(string keyName);

        bool IndexesExists(IEnumerable<string> keyNames);

        void DropIndex(string keyName);

        void DropIndexByName(string indexName);

        void DropIndexes(IEnumerable<string> keyNames);

        void CreateIndex(string keyName);

        void CreateIndexes(IEnumerable<string> keyNames);

        void CreateIndexes(IMongoIndexKeys keys, IMongoIndexOptions options);

        void ReIndex();

        #endregion
    }
}
