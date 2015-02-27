using System;
using System.Linq;
using System.Diagnostics;
using Ucoin.Log.IServices;
using Ucoin.Framework.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Ucoin.Log.Entities;

namespace Ucoin.Framework.Logging.EntLib
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class MongoDbTraceListener : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache,
           string source, TraceEventType eventType, int id, object data)
        {
            this.Write(data);
        }

        public override void Write(object obj)
        {
            var log = obj as LogEntry;
            if (log == null)
            {
                return;
            }

            var logService = ServiceLocator.GetService<ILogService>(); //採用WCF訪問Log服務

            var appLog = GenerateAppLog(log);
            if (appLog.LogLevelType == LogLevelType.Error || appLog.LogLevelType == LogLevelType.Fatal)
            {
                var errorLog = GenerateErrorLog(appLog, log);
                logService.LogErrorInfo(errorLog);
            }
            else
            {
                logService.LogAppInfo(appLog);
            }
        }

        private ErrorLog GenerateErrorLog(AppLog appLog, LogEntry log)
        {
            var errorLog = appLog.ToEntity<AppLog, ErrorLog>();
            errorLog.AppDomainName = log.AppDomainName;
            errorLog.ErrorMessages = log.ErrorMessages;
            errorLog.MachineName = log.MachineName;
            errorLog.Priority = log.Priority;
            errorLog.ProcessId = log.ProcessId;
            errorLog.ProcessName = log.ProcessName;
            errorLog.Win32ThreadId = log.Win32ThreadId;
            errorLog.ManagedThreadName = log.ManagedThreadName;
            return errorLog;
        }

        private AppLog GenerateAppLog(LogEntry log)
        {
            var appLog = new AppLog();          

            if (log.ExtendedProperties.ContainsKey(typeof(LogModel).Name))
            {
                var logModel = log.ExtendedProperties[typeof(LogModel).Name] as LogModel;
                if (logModel != null)
                {
                    appLog = logModel.ToEntity<LogModel, AppLog>();
                }
            }
            appLog.TimeStamp = log.TimeStamp;
            appLog.Category = log.Categories.FirstOrDefault();

            return appLog;
        }

        public override void Write(string message)
        {
            // no operation
        }

        public override void WriteLine(string message)
        {
            // no operation
        }
    }
}
