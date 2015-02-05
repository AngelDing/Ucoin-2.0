using MongoDB.Bson.Serialization.Attributes;
using System;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.MongoRepository.Test
{
    public class CustomEntityTest : IntKeyMongoEntity
    {
        public string Name { get; set; }
    }
}
