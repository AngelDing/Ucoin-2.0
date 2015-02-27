using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System.Collections.Specialized;
using System.Diagnostics;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.Logging.EntLib
{
    public class EntLibLoggerAdapter : BaseLoggerAdapter
    {
        public LogWriter LogWriter { get; private set; }

        public EntLibLoggerAdapter(NameValueCollection properties)
            : base()
        {
            var logLevel = properties.Get("level").ToEnum(LogLevelType.All);
            var traceEventType = LoggerHelper.GetTraceEventType(logLevel);
            var filter = new SeverityFilter(traceEventType);

            LogWriter = LoggerHelper.BuildLogWriter(filter);
        }

        protected override ILogger CreateLogger(string name)
        {
            return new EntLibLogger(name, LogWriter);
        }
    }
}