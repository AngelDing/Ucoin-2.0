using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using Ucoin.Framework.Entities;
using Ucoin.Framework.MongoRepository.Conventions;

namespace Ucoin.Framework.MongoRepository
{
    public static class MongoInitHelper
    {
        private static readonly object lockObj = new object();
        private static bool isInited = false;

        /// <summary>
        /// 默認的Mongo映射策略，比如日期顯示，Id生成策略；
        /// 用戶自定義的策略，請在各自調用端實現注入，且保證只注入一次
        /// </summary>
        public static void InitMongoDBRepository()
        {
            lock (lockObj)
            {
                if (!isInited)
                {
                    isInited = true;
                    RegisterConventions();
                    RegisterClassMap();
                }
            }
        }

        private static void RegisterClassMap()
        {
            BsonClassMap.RegisterClassMap<CommonMongoEntity>(rc =>
            {
                rc.AutoMap();
                rc.SetIdMember(rc.GetMemberMap(c => c.Id));
                rc.IdMemberMap.SetRepresentation(BsonType.ObjectId);
                rc.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
            });          
        }

        private static void RegisterConventions()
        {
            var conventionPack = new ConventionPack();

            conventionPack.Add(new UseLocalDateTimeConvention());

            ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
        }
    }
}
