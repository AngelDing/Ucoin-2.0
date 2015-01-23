using MongoDB.Driver;
using System;
using System.Configuration;
using Ucoin.Framework.MongoRepository;

namespace Ucoin.MongoRepository.Test
{
    public abstract class BaseMongoTest : IDisposable
    {
        public BaseMongoTest()
        {
            MongoInitHelper.InitMongoDBRepository();
            Dispose();
        }

        public virtual void Dispose()
        {
            var connectionString = ConfigurationManager
                .ConnectionStrings["MongoTestDB"].ConnectionString;
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            client.GetServer().DropDatabase(url.DatabaseName);
        }
    }
}
