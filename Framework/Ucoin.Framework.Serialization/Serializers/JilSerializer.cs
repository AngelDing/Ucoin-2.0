using Jil;
using System;
using System.Threading.Tasks;

namespace Ucoin.Framework.Serialization
{
	public class JilSerializer : ISerializer
	{
        public object Serialize(object item)
        {
            var jsonString = JSON.Serialize(item, new Options(includeInherited: true));
            return jsonString;
        }

        public Task<object> SerializeAsync(object item)
		{
			return Task.Factory.StartNew(() => Serialize(item));
		}

        public object Deserialize(object serializedObject, Type type)
		{
            var jsonString = serializedObject.ToString();
            return JSON.Deserialize(jsonString, type);
		}

        public Task<object> DeserializeAsync(object serializedObject, Type type)
		{
			return Task.Factory.StartNew(() => Deserialize(serializedObject, type));
		}

        public T Deserialize<T>(object serializedObject)
		{
            var jsonString = serializedObject.ToString();
			return JSON.Deserialize<T>(jsonString);
		}

        public Task<T> DeserializeAsync<T>(object serializedObject)
		{
			return Task.Factory.StartNew(() => Deserialize<T>(serializedObject));
		}

        public SerializationFormat Format
        {
            get { return SerializationFormat.Jil; }
        }
    }
}