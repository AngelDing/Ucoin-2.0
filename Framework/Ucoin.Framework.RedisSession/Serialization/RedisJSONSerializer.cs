using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
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
        private static ConcurrentDictionary<string, Type> TypeCache = 
            new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Format string used to write type information into the Redis entry before the JSON data
        /// </summary>
        protected string typeInfoPattern = "|!a_{0}_a!|";
        /// <summary>
        /// Regular expression used to extract type information from Redis entry
        /// </summary>
        protected Regex typeInfoReg = new Regex(@"\|\!a_(.*)_a\!\|", RegexOptions.Compiled);

        /// <summary>
        /// Internal dictionaries for type info for commonly used types, which allows for slightly shorter
        ///     type names in the serialized output
        /// </summary>
        protected readonly Dictionary<Type, string> TypeInfoShortcutsSrlz = new Dictionary<Type, string>() 
        { 
            { typeof(int), "SysInt" },
            { typeof(string), "SysString" },
            { typeof(long), "SysLong" },
            { typeof(double), "SysDouble" },
            { typeof(float), "SysFloat" },
            { typeof(int[]), "SysIntArr" },
            { typeof(string[]), "SysStringArr" },
            { typeof(DateTime), "SysDateTime" },
            { typeof(bool), "SysBool" },
            { typeof(byte), "SysByte" }
        };

        /// <summary>
        /// Internal dictionaries for type info for commonly used types, which allows for slightly shorter
        ///     type names in the serialized output
        /// </summary>
        protected readonly Dictionary<string, Type> TypeInfoShortcutsDsrlz = new Dictionary<string, Type>() 
        { 
            { "SysInt", typeof(int) },
            { "SysString", typeof(string) },
            { "SysLong", typeof(long) },
            { "SysDouble", typeof(double) },
            { "SysFloat", typeof(float) },
            { "SysIntArr", typeof(int[]) },
            { "SysStringArr", typeof(string[]) },
            { "SysDateTime", typeof(DateTime) },
            { "SysBool", typeof(bool) },
            { "SysByte", typeof(byte) }
        };

        // ADO.NET serialization is difficult because of the recursive nature of the datastructures. In order
        //      to support DataTable and DataSet serialization, we keep track of their type names and if a
        //      Session value is one of these, we use the standard XML serializer for it instead.
        protected string DataTableTypeSerialized = typeof(DataTable).FullName;
        protected string DataSetTypeSerialized = typeof(DataSet).FullName;

        /// <summary>
        /// Deserializes a string containing type and object information back into the original object
        /// </summary>
        /// <param name="objRaw">A string containing type info and JSON object data</param>
        /// <returns>The original object</returns>
        public virtual object DeserializeOne(string objRaw)
        {
            Match fieldTypeMatch = this.typeInfoReg.Match(objRaw);

            if (fieldTypeMatch.Success)
            {
                // if we are deserializing a datatable, use this
                if (fieldTypeMatch.Groups[1].Value == DataTableTypeSerialized)
                {
                    DataSet desDtWrapper = new DataSet();
                    using (StringReader rdr = new StringReader(objRaw.Substring(fieldTypeMatch.Length)))
                    {
                        desDtWrapper.ReadXml(rdr);
                    }
                    return desDtWrapper.Tables[0];

                }
                // or if we are doing a dataset
                else if (fieldTypeMatch.Groups[1].Value == DataSetTypeSerialized)
                {
                    DataSet dsOut = new DataSet();
                    using (StringReader rdr = new StringReader(objRaw.Substring(fieldTypeMatch.Length)))
                    {
                        dsOut.ReadXml(rdr);
                    }
                    return dsOut;
                }
                // or for most things that are sane, use this
                else
                {
                    string typeInfoString = fieldTypeMatch.Groups[1].Value;
                    Type typeData;

                    if (this.TypeInfoShortcutsDsrlz.ContainsKey(typeInfoString))
                    {
                        typeData = this.TypeInfoShortcutsDsrlz[typeInfoString];
                    }
                    else if(RedisJsonSerializer.TypeCache.TryGetValue(typeInfoString, out typeData))
                    {
                        // great, we have it in cache
                    }
                    else
                    {
                        typeData = innerSerializer.Deserialize<Type>(typeInfoString);

                        #region tryCacheTypeInfo
                        try
                        {
                            // we should cache it for future use
                            TypeCache.AddOrUpdate(
                                typeInfoString,
                                typeData,
                                (str, existing) => typeData); // replace with our type data if already exists
                        }
                        catch(Exception cacheExc)
                        {
                            RedisSerializationConfig.SerializerExceptionLoggingDel(
                                new TypeCacheException(
                                    string.Format(
                                        "Unable to cache type info for raw value '{0}' during deserialization",
                                        objRaw), 
                                    cacheExc));
                        }
                        #endregion
                    }

                    return innerSerializer.Deserialize(objRaw.Substring(fieldTypeMatch.Length),typeData);
                }
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Serializes one key and object into a string containing type and JSON data
        /// </summary>
        /// <param name="key">The key of the object in the Session, does not factor into the
        ///     output except in instances of ADO.NET serialization</param>
        /// <param name="origObj">The value of the Session property</param>
        /// <returns>A string containing type information and JSON data about the object, or XML data
        ///     in the case of serialiaing ADO.NET objects. Don't store ADO.NET objects in Session if 
        ///     you can help it, but if you do we don't want to mess up your Session</returns>
        public virtual string SerializeOne(string key, object origObj)
        {
            // ServiceStack JSONSerializer incapable of serializing datatables... not that we should be storing
            //      any but Dustin code will occasionally
            if (origObj is DataTable)
            {
                DataTable dtToStore = origObj as DataTable;
                // in order to write to xml the TableName property must be set
                if (string.IsNullOrEmpty(dtToStore.TableName))
                {
                    dtToStore.TableName = key + "-session-datatable";
                }
                StringBuilder xmlSer = new StringBuilder();
                using (StringWriter xmlSw = new StringWriter(xmlSer))
                {
                    dtToStore.WriteXml(xmlSw, XmlWriteMode.WriteSchema);
                }

                return string.Format(typeInfoPattern, DataTableTypeSerialized) + xmlSer.ToString();
            }
            // the same is true of DataSet as DataTable
            else if (origObj is DataSet)
            {
                StringBuilder xmlSer = new StringBuilder();
                using (StringWriter xmlSw = new StringWriter(xmlSer))
                {
                    DataSet dsToStore = origObj as DataSet;
                    dsToStore.WriteXml(xmlSw, XmlWriteMode.WriteSchema);
                }

                return string.Format(typeInfoPattern, DataSetTypeSerialized) + xmlSer.ToString();
            }
            else
            {
                Type objType = origObj.GetType();
                string typeInfo;

                if(TypeInfoShortcutsSrlz.ContainsKey(objType))
                {
                    typeInfo = TypeInfoShortcutsSrlz[objType];
                }
                else
                {
                    typeInfo = innerSerializer.SerializeToString(objType);
                }
                
                string objInfo = innerSerializer.SerializeToString(origObj);

                return string.Format(this.typeInfoPattern, typeInfo) + objInfo;
            }
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