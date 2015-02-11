using System;
using Ucoin.Framework.Logging;

namespace Ucoin.Logging.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapter = new ConsoleOutLoggerDemo().GetLoggerFactoryAdapter();
            LogManager.Adapter = adapter;

            var logger = LogManager.GetLogger("console");
            logger.Debug("debug");
            logger.Trace("trace");
            logger.Info("info");
            logger.Warn("warn");
            logger.Error("error");
            logger.Fatal("fatal");

            Console.ReadLine();
        }
    }
}
