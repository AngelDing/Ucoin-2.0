using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ucoin.Framework.Entities
{
    public class CommonMongoEntity : BaseMongoEntity, IEntity<string>
    {
        public string Id { get; set; }
    }
}
