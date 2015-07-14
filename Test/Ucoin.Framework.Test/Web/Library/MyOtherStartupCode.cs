using System;
using Ucoin.Framework.Test.Web.Library;
using Ucoin.Framework.Web.Activator;

[assembly: PreApplicationStartMethod(typeof(MyOtherStartupCode), "Start", Order = 2)]
[assembly: PreApplicationStartMethod(typeof(MyOtherStartupCode), "Start2", Order = 4)]

namespace Ucoin.Framework.Test.Web.Library
{
    public static class MyOtherStartupCode
    {
        public static bool StartCalled { get; set; }
        public static bool Start2Called { get; set; }

        internal static void Start()
        {
            if (StartCalled)
            {
                throw new Exception("Unexpected second call to Start");
            }

            StartCalled = true;
            ExecutionLogger.ExecutedOrder += "OtherStart";
        }

        public static void Start2()
        {
            if (Start2Called)
            {
                throw new Exception("Unexpected second call to Start2");
            }

            Start2Called = true;
            ExecutionLogger.ExecutedOrder += "OtherStart2";
        }
    }
}
