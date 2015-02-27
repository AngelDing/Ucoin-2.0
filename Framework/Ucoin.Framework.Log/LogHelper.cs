using System.Threading.Tasks;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.Logging
{
    public static  class LogHelper
    {
        public static void Log(LogModel model)
        {
            Task.Factory.StartNew(() =>
            {
                RealLog(model);
            });
        }

        private static void RealLog(LogModel model)
        {
            var logger = LogManager.GetLogger(model.AppCodeType.GetDescription());

            switch (model.LogLevelType)
            {
                case LogLevelType.Debug:
                    logger.Debug(model);
                    break;
                case LogLevelType.Trace:
                    logger.Trace(model);
                    break;
                case LogLevelType.Info:
                    logger.Info(model);
                    break;
                case LogLevelType.Warn:
                    logger.Warn(model);
                    break;
                case LogLevelType.Error:
                    logger.Error(model);
                    break;
                case LogLevelType.Fatal:
                    logger.Fatal(model);
                    break;
                case LogLevelType.Off:
                    break;
                default:
                    logger.Info(model);
                    break;
            }
        }
    }
}
