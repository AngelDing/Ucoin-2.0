using System;

namespace Ucoin.Framework.ServiceLocator.Test
{
    public class AdvancedLogger : IUnregisteredLogger
    {
        public void Log(string msg)
        {
            Console.WriteLine("Log: {0}", msg);
        }
    }
}