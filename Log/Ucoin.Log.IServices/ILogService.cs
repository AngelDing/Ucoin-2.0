
using Ucoin.Log.Entities;

namespace Ucoin.Log.IServices
{
    public interface ILogService
    {
        /// <summary>
        /// 程序運行時，記錄的相關日誌，包括人工捕獲的錯誤日誌
        /// </summary>
        /// <param name="log"></param>
        void LogAppInfo(AppLog log);

        /// <summary>
        /// 程序未捕獲的錯誤日誌
        /// </summary>
        /// <param name="log"></param>
        void LogErrorInfo(ErrorLog log);

        /// <summary>
        /// 性能日誌
        /// </summary>
        /// <param name="log"></param>
        void LogPerfInfo(PerfLog log);
    }
}
