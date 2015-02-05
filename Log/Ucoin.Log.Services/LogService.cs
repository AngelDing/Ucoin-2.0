using System;
using Ucoin.Framework.MongoDb;
using Ucoin.Log.Entities;
using Ucoin.Log.IServices;

namespace Ucoin.Log.Services
{
    public class LogService : BaseLogService, ILogService
    {
        public void LogAppInfo(AppLog log)
        {
            try
            {
                var logRepo = new UcoinLogMongoDb<AppLog>();
                logRepo.Insert(log);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void LogErrorInfo(ErrorLog log)
        {
            try
            {
                var logRepo = new UcoinLogMongoDb<ErrorLog>();
                logRepo.Insert(log);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void LogPerfInfo(PerfLog log)
        {
            try
            {
                var logRepo = new UcoinLogMongoDb<PerfLog>();
                logRepo.Insert(log);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
