using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ucoin.Framework.Entities
{
    public class StringKeyMongoEntity : BaseMongoEntity, IAggregateRoot<string>
    {
        public string Id { get; set; }
    }
}
