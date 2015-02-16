using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ucoin.Framework.Logging.EntLib
{
    public class EntLibLogger  : BaseLogger
    {
        private class TraceLevelLogEntry : LogEntry
        {
            public TraceLevelLogEntry(string category, TraceEventType severity)
            {
                Categories.Add(category);
                Severity = severity;               
            }
        }

        private readonly LogEntry VerboseLogEntry;
        private readonly LogEntry InformationLogEntry;
        private readonly LogEntry WarningLogEntry;
        private readonly LogEntry ErrorLogEntry;
        private readonly LogEntry CriticalLogEntry;

        private readonly string category;
        private readonly LoggerSettings settings;
        private readonly LogWriter logWriter;

        public string Category
        {
            get { return category; }
        }

        public LoggerSettings Settings
        {
            get { return settings; }
        }

        public LogWriter LogWriter
        {
            get { return logWriter; }
        }

        public EntLibLogger(string category, LogWriter logWriter, LoggerSettings settings)
        {
            this.category = category;
            this.logWriter = logWriter;
            this.settings = settings;

            VerboseLogEntry = new TraceLevelLogEntry(category, TraceEventType.Verbose);
            InformationLogEntry = new TraceLevelLogEntry(category, TraceEventType.Information);
            WarningLogEntry = new TraceLevelLogEntry(category, TraceEventType.Warning);
            ErrorLogEntry = new TraceLevelLogEntry(category, TraceEventType.Error);
            CriticalLogEntry = new TraceLevelLogEntry(category, TraceEventType.Critical);
        }

        #region IsXXXXEnabled

        public override bool IsTraceEnabled
        {
            get { return ShouldLog(VerboseLogEntry); }
        }

        public override bool IsDebugEnabled
        {
            get { return ShouldLog(VerboseLogEntry); }
        }

        public override bool IsInfoEnabled
        {
            get { return ShouldLog(InformationLogEntry); }
        }

        public override bool IsWarnEnabled
        {
            get { return ShouldLog(WarningLogEntry); }
        }

        public override bool IsErrorEnabled
        {
            get { return ShouldLog(ErrorLogEntry); }
        }

        public override bool IsFatalEnabled
        {
            get { return ShouldLog(CriticalLogEntry); }
        }

        #endregion
        

        protected override void Write(LogLevel logLevel, object message, Exception exception)
        {
            LogEntry log = CreateLogEntry(GetTraceEventType(logLevel));

            if (ShouldLog(log))
            {
                PopulateLogEntry(log, message, exception);
                WriteLog(log);
            }
        }

        protected virtual bool ShouldLog(LogEntry log)
        {
            return logWriter.ShouldLog(log);
        }

        protected virtual void WriteLog(LogEntry log)
        {
            logWriter.Write(log);
        }

        protected virtual TraceEventType GetTraceEventType(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return TraceEventType.Verbose;
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
                case LogLevel.Off:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }
        }

        protected virtual LogEntry CreateLogEntry(TraceEventType traceEventType)
        {
            LogEntry log = new LogEntry();
            log.Categories.Add(category);
            log.Priority = settings.priority;
            log.Severity = traceEventType;
            return log;
        }

        protected virtual void PopulateLogEntry(LogEntry log, object message, Exception ex)
        {
            log.Message = (message == null ? null : message.ToString());
            if (ex != null)
            {
                AddExceptionInfo(log, ex);
            }
        }

        protected virtual void AddExceptionInfo(LogEntry log, Exception exception)
        {
            if (exception != null && settings.exceptionFormat != null)
            {
                string errorMessage = settings.exceptionFormat
                    .Replace("$(exception.message)", exception.Message)
                    .Replace("$(exception.source)", exception.Source)
                    .Replace("$(exception.targetsite)", (exception.TargetSite==null)?string.Empty:exception.TargetSite.ToString())
                    .Replace("$(exception.stacktrace)", exception.StackTrace)
                    ;
                //                StringBuilder sb = new StringBuilder(128);
                //                sb.Append("Exception[ ");
                //                sb.Append("message = ").Append(exception.Message).Append(separator);
                //                sb.Append("source = ").Append(exception.Source).Append(separator);
                //                sb.Append("targetsite = ").Append(exception.TargetSite).Append(separator);
                //                sb.Append("stacktrace = ").Append(exception.StackTrace).Append("]");
                //                return sb.ToString();
                log.AddErrorMessage(errorMessage);
            }
        }
    }
}