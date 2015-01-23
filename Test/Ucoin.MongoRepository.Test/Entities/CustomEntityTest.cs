using MongoDB.Bson.Serialization.Attributes;
using System;
using Ucoin.Framework.Entities;
using Ucoin.Framework.MongoRepository.IdGenerators;

namespace Ucoin.MongoRepository.Test
{
    public class CustomEntityTest : IntKeyMongoEntity
    {
        public string Name { get; set; }
    }
}
