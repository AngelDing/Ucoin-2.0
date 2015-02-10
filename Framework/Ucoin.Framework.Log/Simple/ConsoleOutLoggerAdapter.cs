using System.Collections.Generic;
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
