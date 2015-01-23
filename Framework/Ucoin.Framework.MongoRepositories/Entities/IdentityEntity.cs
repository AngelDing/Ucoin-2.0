using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ucoin.Framework.Entities
{
    [CollectionName("IdentityEntity")]
    public class IdentityEntity<T> : StringKeyMongoEntity
    {
        public string Key
        {
            get;
            set;
        }
        public T Value
        {
            get;
            set;
        }
    }   
}
