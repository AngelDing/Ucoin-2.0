using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Ucoin.Framework.Logging.EntLib
{
    public class CustomTraceListener : FormattedTraceListenerBase
    {
        private class LogEntryMessage
        {
            private readonly ILogFormatter logFormatter;
            private readonly LogEntry logEntry;
            private string cachedResult;

            public LogEntryMessage(ILogFormatter formatter, LogEntry entry)
            {
                logFormatter = formatter;
                logEntry = entry;
            }

            public override string ToString()
            {
                if (cachedResult == null)
                {
                    if (logFormatter == null)
                    {
                        cachedResult = logEntry.ToString();
                    }
                    else
                    {
                        cachedResult = logFormatter.Format(logEntry);
                    }
                }
                return cachedResult;
            }
        }

        private readonly string _loggerNameFormat = "{listenerName}.{sourceName}";
        private string _loggerName;

        public CustomTraceListener(CustomTraceListenerData data, ILogFormatter formatter)
            :base(formatter)
        {
            if (data.LoggerNameFormat != null)
            {
                _loggerNameFormat = data.LoggerNameFormat;
            }
            _loggerName = data.Name;
        }

        public string LoggerNameFormat
        {
            get { return _loggerNameFormat; }
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    data = new LogEntryMessage(base.Formatter, (LogEntry)data);
                }
                Log(eventType, source, id, "{0}", data);
            }
        }


        protected virtual void Log(TraceEventType eventType, string source, int id, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = this.LoggerNameFormat.Replace("{listenerName}", _loggerName).Replace("{sourceName}", source);
            }

            var log = LogManager.GetLogger(source);
            LogLevel logLevel = MapLogLevel(eventType);

            switch (logLevel)
            {
                case LogLevel.Trace:
                    log.Trace(format, args);
                    break;
                case LogLevel.Debug:
                    log.Debug(format, args);
                    break;
                case LogLevel.Info:
                    log.Info(format, args);
                    break;
                case LogLevel.Warn:
                    log.Warn(format, args);
                    break;
                case LogLevel.Error:
                    log.Error(format, args);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(format, args);
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eventType", eventType, "invalid TraceEventType value");
            }
        }

        private LogLevel MapLogLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return LogLevel.Trace;
                case TraceEventType.Verbose:
                    return LogLevel.Debug;
                case TraceEventType.Information:
                    return LogLevel.Info;
                case TraceEventType.Warning:
                    return LogLevel.Warn;
                case TraceEventType.Error:
                    return LogLevel.Error;
                case TraceEventType.Critical:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Trace;
            }
        }
    }
}