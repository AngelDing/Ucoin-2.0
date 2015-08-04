using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ucoin.Framework.Serialization;

namespace Ucoin.Framework.RedisSession
{
    public class RedisJsonSerializer : IRedisSerializer
    {
        private readonly ISerializer innerSerializer;
        public RedisJsonSerializer()
        {
            innerSerializer = SerializationHelper.Jil;
        }

        private static ConcurrentDictionary<string, Type> TypeCache = new ConcurrentDictionary<string, Type>();
        
        protected string typeInfoPattern = "|!a_{0}_a!|";
        protected Regex typeInfoReg = new Regex(@"\|\!a_(.*)_a\!\|", RegexOptions.Compiled);

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

        private class TypeCacheException : Exception
        {
            public TypeCacheException(string msg, Exception inner) 
                : base(msg, inner)
            {
            }
        }
    }
}