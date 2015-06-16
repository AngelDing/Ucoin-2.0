using System;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Ucoin.Framework.MongoDb.Repositories.Conventions
{
    public class UseLocalDateTimeConvention : IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            IBsonSerializer options = null;
            switch (memberMap.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberMap.MemberInfo;
                    options = GetBsonSerializer(propertyInfo.PropertyType);                  
                    break;
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberMap.MemberInfo;
                    options = GetBsonSerializer(fieldInfo.FieldType);    
                    break;
                default:
                    break;
            }
            memberMap.SetSerializer(options);
        }

        private IBsonSerializer GetBsonSerializer(Type type)
        {
            IBsonSerializer serializer = null;
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                serializer = new DateTimeSerializer(DateTimeKind.Local);
            }
            return serializer;
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }
    }
}
