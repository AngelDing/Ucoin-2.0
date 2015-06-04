namespace Ucoin.Framework.RedisSession
{   
    /// <summary>
    /// An interface containing the methods used by the RedisSessionProvider to convert objects to strings
    ///     or byte arrays, to be written to Redis. Currently, RedisSessionProvider only calls DeserializeOne
    ///     and SerializeWithDirtyChecking methods, though implementing the other methods will be helpful
    ///     during debugging. To set your own custom Serializer for RedisSessionProvider, set the 
    ///     RedisSessionProvider.Config.RedisSerializationConfig.SessionDataSerializer property to an 
    ///     instance of your Serializer class.
    /// </summary>
    public interface IRedisSerializer
    {
        /// <summary>
        /// Deserializes one string into the corresponding, correctly typed object
        /// </summary>
        /// <param name="objRaw">A string of the object data</param>
        /// <returns>The Deserialized object, or null if the string is null or empty</returns>
        object DeserializeOne(string objRaw);

        /// <summary>
        /// This method serializes one key-object pair into a string.
        /// </summary>
        /// <param name="origObj">The value of the Session property</param>
        /// <returns>The serialized origObj data as a string</returns>
        string SerializeOne(object origObj);
    }
}
