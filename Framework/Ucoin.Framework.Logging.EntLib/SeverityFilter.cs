using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Ucoin.Framework.Logging.EntLib
{
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class SeverityFilter : LogFilter
    {
        private int severityMask = Int32.MaxValue;

        public int SeverityMask
        {
            get { return severityMask; }
        }

        public SeverityFilter(TraceEventType severityMask, string name = "Severity Filter")
            : base(name)
        {
            this.severityMask = (int)severityMask;
        }

        public override bool Filter(LogEntry log)
        {
            return ShouldLog(log.Severity);
        }

        public bool ShouldLog(TraceEventType severity)
        {
            int evSeverity = (int)severity;

            return ((evSeverity & severityMask) == evSeverity);
        }
    }
}
