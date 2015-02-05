using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.MongoDb.Entities
{
    public class StringKeyMongoEntity : BaseMongoEntity, IAggregateRoot<string>
    {
        public string Id { get; set; }
    }
}
