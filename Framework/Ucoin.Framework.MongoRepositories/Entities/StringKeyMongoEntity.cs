using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.MongoDb.Entities
{
    public class StringKeyMongoEntity : BaseMongoEntity, IAggregateRoot<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
