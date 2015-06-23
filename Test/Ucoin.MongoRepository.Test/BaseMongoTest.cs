using MongoDB.Driver;
using System;
using System.Configuration;
using Ucoin.Framework.MongoDb.Repositories;
using Ucoin.Framework.Utils;

namespace Ucoin.MongoRepository.Test
{
    public abstract class BaseMongoTest : IDisposable
    {
        public BaseMongoTest()
        {
            MongoInitHelper.InitMongoDBRepository();
        }

        public virtual void Dispose()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoTestDB"].ConnectionString;
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            AsyncHelper.RunSync(() => client.DropDatabaseAsync(url.DatabaseName));
        }
    }
}
