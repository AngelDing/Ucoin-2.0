

using System.Collections.Specialized;
using Ucoin.Framework.Logging.Configuration;
namespace Ucoin.Framework.Logging.Simple
{
    public class DebugLoggerAdapter : BaseSimpleLoggerAdapter
    {
        public DebugLoggerAdapter()
            : base((NameValueCollection)null)
        { 
        }

        public DebugLoggerAdapter(NameValueCollection properties)
            : base(properties)
        {
        }

        public DebugLoggerAdapter(LogArgumentEntity argEntity)
            : base(argEntity)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DebugOutLogger"/> instance.
        /// </summary>
        protected override ILogger CreateLogger(LogArgumentEntity argEntity)
        {
            var log = new DebugOutLogger(argEntity);
            return log;
        }
    }
}
