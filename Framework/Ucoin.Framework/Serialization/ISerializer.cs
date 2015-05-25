namespace Ucoin.Framework.Serialization
{
    using System;

    public interface ISerializer
    {
        SerializationFormat Format { get; }

        string Serialize(object input);

        T Deserialize<T>(string input);

        object Deserialize(string input, Type type);
    }
}
