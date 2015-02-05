using System;
using System.Configuration;
using Ucoin.Framework.Entities;
using Ucoin.Framework.MongoDb.Entities;
using Ucoin.Framework.MongoDb.Managers;
using Ucoin.Framework.MongoDb.Repositories;


namespace Ucoin.MongoRepository.Test
{
    public class MongoTestDB<T, TKey> : MongoRepository<T, TKey> where T : BaseMongoEntity, IAggregateRoot<TKey>//<TKey>
    {
        public MongoTestDB()
            : base(ConfigurationManager.ConnectionStrings["MongoTestDB"].ConnectionString)
        {
        }
    }

    public class MongoTestDB<T> : MongoTestDB<T, string> where T : StringKeyMongoEntity
    {
    }

    public class MongoIndexManagerTest<T> : IndexManager<T> where T : BaseMongoEntity
    {
        public MongoIndexManagerTest()
            : base(ConfigurationManager.ConnectionStrings["MongoTestDB"].ConnectionString)
        {
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
    }
}
