using System;
using System.Diagnostics;
using System.Text;

namespace Ucoin.Framework.Logging.Simple
{
    /// <summary>
    /// Logger sending everything to the trace output stream using System.Diagnostics.Trace.
    /// </summary>
    /// <remarks>
    /// Beware not to use LoggingTraceListener in combination with this logger as 
    /// this would result in an endless loop for obvious reasons!
    /// </remarks>
    public class TraceLogger : BaseSimpleLogger
    {
        private readonly bool useTraceSource;
        private TraceSource traceSource;

        private class FormatOutputMessage
        {
            private readonly TraceLogger outer;
            private readonly LogLevel level;
            private readonly object message;
            private readonly Exception ex;

            public FormatOutputMessage(TraceLogger outer, LogLevel level, object message, Exception ex)
            {
                this.outer = outer;
                this.level = level;
                this.message = message;
                this.ex = ex;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                outer.FormatOutput(sb, level, message, ex);
                return sb.ToString();
            }
        }

        public TraceLogger(bool useTrace, LogArgumentEntity argEntity)
            : base(argEntity)
        {
            useTraceSource = useTrace;
            if (useTraceSource)
            {
                traceSource = new TraceSource(argEntity.LogName, Map2SourceLevel(argEntity.Level));
            }
        }

        protected override bool IsLevelEnabled(LogLevel level)
        {
            if (!useTraceSource)
            {
                return base.IsLevelEnabled(level);
            }
            return traceSource.Switch.ShouldTrace(Map2TraceEventType(level));
        }

        protected override void Write(LogLevel level, object message, Exception e)
        {
            var msg = new FormatOutputMessage(this, level, message, e);
            if (traceSource != null)
            {
                traceSource.TraceEvent(Map2TraceEventType(level), 0, "{0}", msg);
            }
            else
            {
                switch (level)
                {
                    case LogLevel.Info:
                        System.Diagnostics.Trace.TraceInformation("{0}", msg);
                        break;
                    case LogLevel.Warn:
                        System.Diagnostics.Trace.TraceWarning("{0}", msg);
                        break;
                    case LogLevel.Error:
                    case LogLevel.Fatal:
                        System.Diagnostics.Trace.TraceError("{0}", msg);
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine(msg);
                        break;
                }
            }
        }

        private TraceEventType Map2TraceEventType(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return TraceEventType.Verbose;
                case LogLevel.Debug:
                    return TraceEventType.Verbose;
                case LogLevel.Info:
                    return TraceEventType.Information;
                case LogLevel.Warn:
                    return TraceEventType.Warning;
                case LogLevel.Error:
                    return TraceEventType.Error;
                case LogLevel.Fatal:
                    return TraceEventType.Critical;
                default:
                    return 0;
            }
        }

        private SourceLevels Map2SourceLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                case LogLevel.Trace:
                    return SourceLevels.All;
                case LogLevel.Debug:
                    return SourceLevels.Verbose;
                case LogLevel.Info:
                    return SourceLevels.Information;
                case LogLevel.Warn:
                    return SourceLevels.Warning;
                case LogLevel.Error:
                    return SourceLevels.Error;
                case LogLevel.Fatal:
                    return SourceLevels.Critical;
                default:
                    return SourceLevels.Off;
            }
        }
    }
}
