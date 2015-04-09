using System;

namespace Ucoin.Framework.ServiceLocation.Test
{
    public class AdvancedLogger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine("Log: {0}", msg);
        }
    }
}