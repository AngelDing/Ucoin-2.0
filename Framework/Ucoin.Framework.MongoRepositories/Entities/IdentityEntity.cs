using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ucoin.Framework.Entities
{
    [CollectionName("IdentityEntity")]
    public class IdentityEntity<T> : CommonMongoEntity
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
