using System.Collections.Generic;
using System.Collections.Specialized;
using Ucoin.Framework.Logging.Configuration;

namespace Ucoin.Framework.Logging.Simple
{
    public class ConsoleOutLoggerAdapter : BaseSimpleLoggerAdapter
    {
        private readonly bool useColor;

        public ConsoleOutLoggerAdapter()
            : base((NameValueCollection)null)
        { 
        }

        public ConsoleOutLoggerAdapter(NameValueCollection properties)
            : base(properties)
        {
        } 

        public ConsoleOutLoggerAdapter(NameValueCollection properties, bool useColor)
            : this(properties)
        {
            this.useColor = useColor;
        }


        public ConsoleOutLoggerAdapter(LogArgumentEntity argEntity)
            : base(argEntity)
        { 
        }


        public ConsoleOutLoggerAdapter(LogArgumentEntity argEntity, bool useColor)
            : this(argEntity)
        {
            this.useColor = useColor;
        }       

        protected override ILogger CreateLogger(LogArgumentEntity argEntity)
        {
            ILogger log = new ConsoleOutLogger(argEntity, this.useColor);
            return log;
        }
    }
}
