using System;
using Ucoin.Framework.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Ucoin.Logging.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("Demo Test");

            logger.Debug("debug");
            logger.Trace("trace");
            logger.Info("info");
            logger.Warn("warn");
            try
            {
                throw new Exception("XXXXXX");
            }
            catch (Exception ex)
            {
                logger.Error("error", ex);
            }
            logger.Fatal("fatal");

            Console.ReadLine();
        }
    }
}
