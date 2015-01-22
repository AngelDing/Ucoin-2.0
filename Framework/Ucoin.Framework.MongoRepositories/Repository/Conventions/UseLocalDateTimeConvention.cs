using System;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace Ucoin.Framework.MongoRepository.Conventions
{
    public class UseLocalDateTimeConvention : IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            IBsonSerializationOptions options = null;
            switch (memberMap.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberMap.MemberInfo;
                    if (propertyInfo.PropertyType == typeof(DateTime) ||
                        propertyInfo.PropertyType == typeof(DateTime?))
                    {
                        options = new DateTimeSerializationOptions(DateTimeKind.Local);
                    }
                    break;
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberMap.MemberInfo;
                    if (fieldInfo.FieldType == typeof(DateTime) ||
                        fieldInfo.FieldType == typeof(DateTime?))
                    {
                        options = new DateTimeSerializationOptions(DateTimeKind.Local);
                    }
                    break;
                default:
                    break;
            }
            memberMap.SetSerializationOptions(options);
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }
    }
}
