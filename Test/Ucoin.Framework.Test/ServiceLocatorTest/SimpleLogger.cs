using System;

namespace Ucoin.Framework.ServiceLocator.Test
{
    public class SimpleLogger : ILogger
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}