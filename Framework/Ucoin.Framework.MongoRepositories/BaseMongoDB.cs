using MongoDB.Driver;
using System;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.Framework.MongoDb
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
            else
            {
                if (typeof(T).IsSubclassOf(typeof(BaseMongoEntity)))
                {
                    this.collection = db.GetCollection<T>(GetCollectionName());
                }
            }
        }

        private static string GetCollectionName()
        {
            string collectionName;
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                collectionName = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionName = typeof(T).Name;
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
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
