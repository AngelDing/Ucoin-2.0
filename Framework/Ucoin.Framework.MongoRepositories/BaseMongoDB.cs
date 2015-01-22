using MongoDB.Driver;
using System;

namespace Ucoin.Framework.MongoRepository
{
    public class BaseMongoDB<T> where T : class
    {
        private MongoCollection<T> collection;
        private MongoDatabase db;

        public BaseMongoDB(string connectionString)
            : this(connectionString, null)
        {
        }

        public BaseMongoDB(string connectionString, string collectionName)
        {
            var mongoUrl = new MongoUrl(connectionString);
            db = GetDatabaseFromUrl(mongoUrl);
            if (!string.IsNullOrEmpty(collectionName))
            {
                this.collection = db.GetCollection<T>(collectionName);
            }
        }

        private MongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            var server = client.GetServer();
            return server.GetDatabase(url.DatabaseName);
        }

        internal MongoCollection<T> Collection
        {
            get { return this.collection; }
        }

        internal MongoDatabase DB
        {
            get { return this.db; }
        }
    }
}
