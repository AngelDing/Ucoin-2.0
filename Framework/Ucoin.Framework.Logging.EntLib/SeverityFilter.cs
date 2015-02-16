using System;
using System.Diagnostics;
using System.Collections.Specialized;
using Ucoin.Framework.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ucoin.Framework.Logging.EntLib
{
    /// <summary>
    /// Filters <see cref="LogEntry"/>s by severity (see <see cref="TraceEventType"/>).
    /// </summary>
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class SeverityFilter : LogFilter
    {
        private int severityMask = Int32.MaxValue;

        /// <summary>
        /// Bitmask to identify severity levels that should be logged.
        /// </summary>
        public int SeverityMask
        {
            get { return severityMask; }
            set { severityMask = value; }
        }

        public SeverityFilter(string name, int severityMask)
            : base(name)
        {
            this.severityMask = severityMask;
        }

        public SeverityFilter(string name, TraceEventType severityMask)
            : this(name, (int)severityMask)
        {
        }

        /// <summary>
        /// Creates a new filter instance
        /// </summary>
        public SeverityFilter(NameValueCollection attributes)
            : base((attributes != null && attributes["name"] != null) ? attributes["name"] : "Severity Filter")
        {
            //this.severityMask = attributes.Get("SeverityMask").ToInt(this.severityMask);
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
