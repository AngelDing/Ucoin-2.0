
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ucoin.Framework.Logging.EntLib
{
    public static class LoggerHelper
    {
        public static TraceEventType GetTraceEventType(LogLevelType logLevel)
        {
            switch (logLevel)
            {
                case LogLevelType.All:                   
                case LogLevelType.Trace:
                case LogLevelType.Debug:
                    return TraceEventType.Verbose | TraceEventType.Information | TraceEventType.Warning
                       | TraceEventType.Error | TraceEventType.Critical;
                case LogLevelType.Info:
                    return TraceEventType.Information | TraceEventType.Warning
                       | TraceEventType.Error | TraceEventType.Critical;
                case LogLevelType.Warn:
                    return TraceEventType.Warning | TraceEventType.Error | TraceEventType.Critical;
                case LogLevelType.Error:
                    return TraceEventType.Error | TraceEventType.Critical;
                case LogLevelType.Fatal:
                    return TraceEventType.Critical;
                case LogLevelType.Off:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }
        }

        public static LogWriter BuildLogWriter(ILogFilter filter)
        {
            var filters = new ILogFilter[] { filter };
            //通用的LogSource，對應的是specialSources的allEvents
            var allEventsTraceSource = BuildAllEventsTraceSource();
            //日誌記錄組件自己出現錯誤時的LogSource，對應的是specialSources的errors
            var errorsLogSource = BuildErrorsLogSource();
            //categorySources, 可不需要設置，全部走allEventsTraceSource            
            var categorySources = new List<LogSource>();
            //notProcessedTraceSource, 由於設置了allEventsTraceSource,故不需要設置
            var notProcessedTraceSource = new LogSource("Unprocessed Category");

            var logWriter = new LogWriter(filters, categorySources, allEventsTraceSource, 
                notProcessedTraceSource,errorsLogSource, "DefaultCategory", true, true);
            return logWriter;
        }

        private static LogSource BuildErrorsLogSource()
        {
            var formatter = new TextFormatter(
                @"Timestamp: {timestamp}
                Category: {category}
                Message: {message}
                Priority: {priority}
                EventId: {eventid}
                Severity: {severity}
                Title:{title}
                Machine: {machine}
                Application Domain: {appDomain}
                Process Id: {processId}
                Process Name: {processName}
                Win32 Thread Id: {win32ThreadId}
                Thread Name: {threadName}
                Extended Properties: {dictionary({key} - {value})}"
            );

            var fileListener = new FlatFileTraceListener("ErrorLog.txt");
            fileListener.Formatter = formatter;
            var listeners = new TraceListener[] { fileListener };

            var source = new LogSource("FlatFileTraceListener", listeners, SourceLevels.All);
            return source;     
        }

        private static LogSource BuildAllEventsTraceSource()
        {
            var listeners = new TraceListener[] { new MongoDbTraceListener() };
            var source = new LogSource("MongoDbTraceListener", listeners, SourceLevels.All);
            return source;            
        }
    }
}
