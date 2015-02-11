using System.Collections.Specialized;
using Ucoin.Framework.Logging;
using Ucoin.Framework.Logging.Simple;

namespace Ucoin.Logging.Demo
{
    public class ConsoleOutLoggerDemo
    {
        public ILoggerAdapter GetLoggerFactoryAdapter()
        {
            return new ConsoleOutLoggerAdapter(CreateProperties(), true);
        }

        protected static NameValueCollection CreateProperties()
        {
            var properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            properties["dateTimeFormat"] = "yyyy/MM/dd HH:mm:ss:fff";      
            return properties;
        }
    }
}
