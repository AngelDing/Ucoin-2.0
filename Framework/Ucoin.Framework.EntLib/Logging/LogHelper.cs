using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.EntLib.Logging
{
    public static class LogHelper
    {
        public static void WriteAppLog(string title, string msg = null)
        {
            //WriteAppLog(title, LogPriority.Normal, msg);
        }

        public static void WriteAppLog(string title, int logPriority, string msg = null)
        {
            //var log = new LogEntry();
            //log.Title = title;
            //log.Categories.Add(LogCategoryType.AppLog.GetDescription());
            //log.Message = msg;
            //log.Priority = logPriority;
            //log.
            //Logger.Write(log);
        }
    }
}
