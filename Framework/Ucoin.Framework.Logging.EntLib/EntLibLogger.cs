using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Text;

namespace Ucoin.Framework.Logging.EntLib
{
    public class EntLibLogger : BaseLogger
    {
        private class TraceLevelLogEntry : LogEntry
        {
            public TraceLevelLogEntry(string category, TraceEventType severity)
            {
                Categories.Add(category);
                Severity = severity;
            }
        }

        private readonly string category;
        private readonly LogWriter logWriter;

        public EntLibLogger(string category, LogWriter logWriter)
        {
            this.category = category;
            this.logWriter = logWriter;
        }

        #region IsXXXXEnabled

        public override bool IsTraceEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Verbose);
                return ShouldLog(logEntry);
            }
        }

        public override bool IsDebugEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Verbose);
                return ShouldLog(logEntry);
            }
        }

        public override bool IsInfoEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Information);
                return ShouldLog(logEntry);
            }
        }

        public override bool IsWarnEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Warning);
                return ShouldLog(logEntry);
            }
        }

        public override bool IsErrorEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Error);
                return ShouldLog(logEntry);
            }
        }

        public override bool IsFatalEnabled
        {
            get
            {
                var logEntry = new TraceLevelLogEntry(category, TraceEventType.Critical);
                return ShouldLog(logEntry);
            }
        }

        #endregion

        protected override void Write(LogLevelType logLevel, object message, Exception exception)
        {
            var traceEventType = LoggerHelper.GetTraceEventType(logLevel);
            var log = new TraceLevelLogEntry(category, traceEventType);
            PopulateLogEntry(log, message, exception);
            logWriter.Write(log);
        }

        private bool ShouldLog(LogEntry log)
        {
            return logWriter.ShouldLog(log);
        }

        private void PopulateLogEntry(LogEntry log, object message, Exception ex)
        {
            if (message == null)
            {
                return;
            }
            if (message is LogModel)
            {
                var logModel = message as LogModel;
                if (logModel != null)
                {
                    log.ExtendedProperties.Add(typeof(LogModel).Name, logModel);
                }
            }
            log.Message =  message.ToString();
            if (ex != null)
            {
                AddExceptionInfo(log, ex);
            }
        }

        private void AddExceptionInfo(LogEntry log, Exception exception)
        {
            var sb = new StringBuilder();
            sb.Append("Exception[ ");
            sb.Append("message = ").Append(exception.Message).Append(";");
            sb.Append("source = ").Append(exception.Source).Append(";");
            sb.Append("targetsite = ").Append(exception.TargetSite).Append(";");
            sb.Append("stacktrace = ").Append(exception.StackTrace).Append(" ]");
            log.AddErrorMessage(sb.ToString());
        }
    }
}