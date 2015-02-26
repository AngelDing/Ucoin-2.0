using System;
using Ucoin.Framework.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ucoin.Logging.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            EntLibLoggerInit();
            var logger = LogManager.GetLogger("Demo Test");
            logger.Debug("debug");
            logger.Trace("trace");
            logger.Info("info");
            logger.Warn("warn");
            logger.Error("error");
            logger.Fatal("fatal");

            Console.ReadLine();
        }

        //採用EntLibLogger時需要初始化
        private static void EntLibLoggerInit()
        {
             Logger.SetLogWriter(new LogWriterFactory().Create());
        }
    }
}
