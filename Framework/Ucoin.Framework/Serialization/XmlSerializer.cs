using System;
using System.IO;
using System.Text;

namespace Ucoin.Framework.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Xml; }
        }

        public object Serialize(object input)
        {
            if (null == input)
            {
                return string.Empty;
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    var xmlSer = new System.Xml.Serialization.XmlSerializer(input.GetType());
                    xmlSer.Serialize(ms, input);
                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public T Deserialize<T>(object input)
        {
            T res = default(T);
            var obj = GetObject(input, typeof(T));
            if (null != obj)
            {
                res = (T)obj;
            }
            return res;
        }

        public object Deserialize(object input, Type type)
        {
            return GetObject(input, type);
        }

        private object GetObject(object input, Type type)
        {
            try
            {
                object res = null;
                var bytes = Encoding.UTF8.GetBytes(input.ToString());
                using (var stream = new MemoryStream(bytes))
                {
                    var xmlSer = new System.Xml.Serialization.XmlSerializer(type);
                    res = xmlSer.Deserialize(stream);
                }
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
