using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ucoin.Framework.Serialization;

namespace Ucoin.Framework.RedisSession
{
    /// <summary>
    /// This serializer encodes/decodes Session values into/from JSON for Redis persistence, using
    ///     the Json.NET library. The only exceptions are for ADO.NET types (DataTable and DataSet),
    ///     which revert to using XML serialization.
    /// </summary>
    public class RedisJsonSerializer : IRedisSerializer
    {
        private readonly ISerializer innerSerializer;
        public RedisJsonSerializer()
        {
            innerSerializer = Serializer.Jil;
        }

        /// <summary>
        /// Shared concurrent dictionary to optimize type-safe deserialization from json, since
        /// we store the type info in the string
        /// </summary>
        private static ConcurrentDictionary<string, Type> TypeCache = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Format string used to write type information into the Redis entry before the JSON data
        /// </summary>
        protected string typeInfoPattern = "|!a_{0}_a!|";
        /// <summary>
        /// Regular expression used to extract type information from Redis entry
        /// </summary>
        protected Regex typeInfoReg = new Regex(@"\|\!a_(.*)_a\!\|", RegexOptions.Compiled);

        /// <summary>
        /// Deserializes a string containing type and object information back into the original object
        /// </summary>
        /// <param name="objRaw">A string containing type info and JSON object data</param>
        /// <returns>The original object</returns>
        public object DeserializeOne(string objRaw)
        {
            Match fieldTypeMatch = this.typeInfoReg.Match(objRaw);

            if (fieldTypeMatch.Success)
            {
                string typeInfoString = fieldTypeMatch.Groups[1].Value;
                Type typeData;

                if (TypeCache.ContainsKey(typeInfoString))
                {
                    if (TypeCache.TryGetValue(typeInfoString, out typeData))
                    {
                        return innerSerializer.Deserialize(objRaw.Substring(fieldTypeMatch.Length), typeData);
                    }
                }
                else
                {
                    var msg =  string.Format("Unable to cache type info for raw value '{0}' during deserialization", objRaw);
                    RedisSerializationConfig.SerializerExceptionLoggingDel(new TypeCacheException(msg,null));
                }
            }
            return null;
        }
        
        /// <summary>
        /// Serializes one key and object into a string containing type and JSON data
        /// </summary>
        /// <param name="origObj">The value of the Session property</param>
        /// <returns>A string containing type information and JSON data about the object, or XML data
        ///     in the case of serialiaing ADO.NET objects. Don't store ADO.NET objects in Session if 
        ///     you can help it, but if you do we don't want to mess up your Session</returns>
        public string SerializeOne(object origObj)
        {
            Type objType = origObj.GetType();
            string typeInfo = objType.FullName;

            if (TypeCache.ContainsKey(typeInfo) == false)
            {
                TypeCache.TryAdd(typeInfo, objType);
            }

            string objInfo = innerSerializer.SerializeToString(origObj);

            return string.Format(this.typeInfoPattern, typeInfo) + objInfo;
        }

        public class TypeCacheException : Exception
        {
            public TypeCacheException(string msg, Exception inner) 
                : base(msg, inner)
            {
            }
        }
    }
}