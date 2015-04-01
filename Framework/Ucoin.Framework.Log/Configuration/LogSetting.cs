using System;
using System.Collections.Specialized;
using Ucoin.Framework.Utils;

namespace Ucoin.Framework.Logging.Configuration
{
    public class LogSetting
    {
        private readonly Type factoryAdapterType = null;
        private readonly NameValueCollection properties = null;

        public LogSetting(Type type, NameValueCollection nvList)
        {
            GuardHelper.ArgumentNotNull(() => type);
            var msg = string.Format("Type {0} does not implement {1}", 
                type.AssemblyQualifiedName, typeof(ILoggerAdapter).FullName);
            GuardHelper.InheritsFrom<ILoggerAdapter>(type, msg);
            
            factoryAdapterType = type;
            properties = nvList;
        }

        public Type FactoryAdapterType
        {
            get { return factoryAdapterType; }
        }

        public NameValueCollection Properties
        {
            get { return properties; } 
        }
    }
}
