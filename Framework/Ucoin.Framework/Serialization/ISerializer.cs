namespace Ucoin.Framework.Serialization
{
    using System;

    public interface ISerializer
    {
        SerializationFormat Format { get; }

        object Serialize(object input);

        T Deserialize<T>(object input);

        object Deserialize(object input, Type type);
    }
}
