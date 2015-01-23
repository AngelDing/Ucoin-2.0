using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.MongoRepository.IdGenerators
{
    public class LongIdGenerator<TDocument, TKey> : IIdGenerator where TDocument : class
    {
        private readonly object lockObject = new object();
        private static LongIdGenerator<TDocument, TKey> _instance = new LongIdGenerator<TDocument, TKey>();

        public static LongIdGenerator<TDocument, TKey> Instance { get { return _instance; } }

        public object GenerateId(object container, object document)
        {
            TKey id = default(TKey);
            var collection = container as MongoCollection;
            if (null != collection)
            {
                var mongoDB = collection.Database; 
                var idColl = mongoDB.GetCollection<IdentityEntity<TKey>>("IdentityEntity");
                var keyName = document.GetType().Name;
                id  = RealGenerateId(idColl, keyName) ;
            }
            return id;
        }

        private TKey RealGenerateId(MongoCollection<IdentityEntity<TKey>> idColl, string keyName)
        {
            TKey id;

            var idQuery = new QueryDocument("Key", BsonValue.Create(keyName));

            var idBuilder = new UpdateBuilder();
            idBuilder.Inc("Value", 1);

            var args = new FindAndModifyArgs();
            args.Query = idQuery;
            args.Update = idBuilder;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;
            args.Upsert = true;

            var result = idColl.FindAndModify(args);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                throw new Exception(result.ErrorMessage);
            }
            id = result.GetModifiedDocumentAs<IdentityEntity<TKey>>().Value;

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
