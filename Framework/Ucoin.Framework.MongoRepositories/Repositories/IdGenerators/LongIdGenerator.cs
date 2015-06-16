using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.Framework.MongoDb.Repositories.IdGenerators
{
    public class LongIdGenerator<TDocument, TKey> : IIdGenerator where TDocument : class
    {
        private readonly object lockObject = new object();
        private static LongIdGenerator<TDocument, TKey> _instance = new LongIdGenerator<TDocument, TKey>();

        public static LongIdGenerator<TDocument, TKey> Instance { get { return _instance; } }

        public object GenerateId(object container, object document)
        {
            TKey id = default(TKey);
            var collection = container as IMongoCollection<object>;
            if (null != collection)
            {
                var mongoDB = collection.Database; 
                var idColl = mongoDB.GetCollection<IdentityEntity<TKey>>("IdentityEntity");
                var keyName = document.GetType().Name;
                id  = RealGenerateId(idColl, keyName) ;
            }
            return id;
        }

        private TKey RealGenerateId(IMongoCollection<IdentityEntity<TKey>> idColl, string keyName)
        {
            TKey id;

            var idBuilder = Builders<IdentityEntity<TKey>>.Update.Inc("Value", 1);

            //var args = new FindAndModifyArgs();
            //args.Query = idQuery;
            //args.Update = idBuilder;
            //args.VersionReturned = FindAndModifyDocumentVersion.Modified;
            //args.Upsert = true;

            var options = new FindOneAndUpdateOptions<IdentityEntity<TKey>>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var result = idColl.FindOneAndUpdateAsync<IdentityEntity<TKey>>(i => i.Key == keyName, idBuilder, options);

            id = result.Result.Value;
            //if (!string.IsNullOrEmpty(result.ErrorMessage))
            //{
            //    throw new Exception(result.ErrorMessage);
            //}
            //id = result.GetModifiedDocumentAs<IdentityEntity<TKey>>().Value;

            return id;
        }        

        public bool IsEmpty(object id)
        {
            if (null == id)
            {
                return false;
            }

            return true;
            //try
            //{
            //    var myId = id a;
            //    return myId <= 0;
            //}
            //catch
            //{
            //    return false;
            //}
        }
    }
}
