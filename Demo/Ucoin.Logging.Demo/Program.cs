using System;
using Ucoin.Framework.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ucoin.Framework;

namespace Ucoin.Logging.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var logModel = new LogModel() 
            {
                Message = "test",
                LogLevelType = LogLevelType.Info,
                Detail = "CXXXXX",
                Source = "Offline",
                AppCodeType = AppCodeType.Authority
            };

            LogHelper.Log(logModel);

            Console.ReadLine();
        }
    }
}
