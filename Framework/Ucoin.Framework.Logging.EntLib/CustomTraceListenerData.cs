using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Ucoin.Framework.Logging.EntLib
{
    public class CustomTraceListenerData : TraceListenerData
    {
        private const string loggerNameFormatProperty = "loggerNameFormat";

        public CustomTraceListenerData()
            : base(typeof(CustomTraceListener))
        {
            ListenerDataType = typeof(CustomTraceListenerData);
        }

        public CustomTraceListenerData(string loggerNameFormat, string formatterName)
            : this("unnamed", loggerNameFormat, formatterName)
        {
        }

        public CustomTraceListenerData(string name, string loggerNameFormat, string formatterName)
            : this(name, typeof(CustomTraceListener), loggerNameFormat, formatterName, TraceOptions.None)
        {
        }

        public CustomTraceListenerData(string name, string loggerNameFormat, string formatterName, TraceOptions traceOutputOptions)
            : this(name, typeof(CustomTraceListener), loggerNameFormat, formatterName, traceOutputOptions)
        {
        }

        public CustomTraceListenerData(string name, Type traceListenerType, string loggerNameFormat, string formatterName, TraceOptions traceOutputOptions)
            : base(name, traceListenerType, traceOutputOptions)
        {
            LoggerNameFormat = loggerNameFormat;
            Formatter = formatterName;
        }


        [ConfigurationProperty(loggerNameFormatProperty, IsRequired = false)]
        public string LoggerNameFormat
        {
            get { return (string) base[loggerNameFormatProperty ] ; }
            set { base[loggerNameFormatProperty] = value; }
        }

        [ConfigurationProperty("formatter", IsRequired = false)]
        public string Formatter
        {
            get { return (string)base["formatter"]; }
            set { base["formatter"] = value; }
        }

        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            return new CustomTraceListener(this, BuildFormatterSafe(settings, Formatter));
        }

    }
}