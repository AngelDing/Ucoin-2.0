using System;
using System.Threading.Tasks;
using Ucoin.Framework.Logging;

namespace Ucoin.Framework.Serialization
{
    public abstract class BaseSerializer<TSerializer> : ISerializer 
        where TSerializer : BaseSerializer<TSerializer>, new()
    {
        private readonly ILogger logger;
        /// <summary>
        /// 日誌記錄器，從配置中獲取LogAdapter
        /// </summary>
        protected ILogger Logger
        {
            get { return this.logger; }
        }

        public SerializationFormat Format
        {
            get
            {
                return GetSerializationFormat();
            }
        }

        internal abstract SerializationFormat GetSerializationFormat();
        internal abstract object DoSerialize(object item);
        internal abstract T DoDeserialize<T>(object serializedObject);
        internal abstract object DoDeserialize(object serializedObject, Type type);

        public BaseSerializer()
        {
            logger = LogManager.GetLogger(typeof(TSerializer));
        }

        public object Serialize(object item)
        {
            object result = null;
            try
            {
                result = DoSerialize(item);
            }
            catch (Exception ex)
            {
                var msg = typeof(TSerializer).Name + " Serialize Error.";
                Logger.Error(msg , ex);
            }
            return result;
        }

        public Task<object> SerializeAsync(object item)
        {
            return Task.FromResult(this.Serialize(item));
        }

        public T Deserialize<T>(object serializedObject)
        {
            T result = default(T);
            try
            {
                result = DoDeserialize<T>(serializedObject);
            }
            catch (Exception ex)
            {
                var msg = typeof(TSerializer).Name + " Deserialize Error.";
                Logger.Error(msg, ex);
            }

            return result;
        }

        public Task<T> DeserializeAsync<T>(object serializedObject)
        {
            return Task.FromResult<T>(Deserialize<T>(serializedObject));
        }

        public object Deserialize(object serializedObject, Type type)
        {
            object result = null;
            try
            {
                result = DoDeserialize(serializedObject, type);
            }
            catch (Exception ex)
            {
                var msg = typeof(TSerializer).Name + " Deserialize Error.";
                Logger.Error(msg, ex);
            }

            return result;
        }

        public Task<object> DeserializeAsync(object serializedObject, Type type)
        {
            return Task.FromResult(Deserialize(serializedObject, type));
        }
    }
}
