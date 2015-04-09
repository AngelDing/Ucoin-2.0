using System;

namespace Ucoin.Framework.ServiceLocation.Test
{
    public class SimpleLogger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}