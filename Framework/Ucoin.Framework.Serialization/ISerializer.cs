namespace Ucoin.Framework.Serialization
{
    using System;
    using System.Threading.Tasks;

    public interface ISerializer
    {
        SerializationFormat Format { get; }

        object Serialize(object item);

        //Task<object> SerializeAsync(object item);

        T Deserialize<T>(object serializedObject);

        //Task<T> DeserializeAsync<T>(object serializedObject);

        object Deserialize(object serializedObject, Type type);

        //Task<object> DeserializeAsync(object serializedObject, Type type);
    }
}
