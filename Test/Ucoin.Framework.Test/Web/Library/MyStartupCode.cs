using System;
using Ucoin.Framework.Test.Web.Library;
using Ucoin.Framework.Web.Activator;

[assembly: PreApplicationStartMethod(typeof(MyStartupCode), "Start", RunInDesigner = true)]
[assembly: PreApplicationStartMethod(typeof(MyStartupCode), "Start2", Order = 3)]
[assembly: PreApplicationStartMethod(typeof(MyStartupCode), "Start3", Order = 1)]
[assembly: PostApplicationStartMethod(typeof(MyStartupCode), "CallMeAfterAppStart")]
[assembly: ApplicationShutdownMethod(typeof(MyStartupCode), "CallMeWhenAppEnds")]

namespace Ucoin.Framework.Test.Web.Library
{
    public static class MyStartupCode
    {
        public static bool StartCalled { get; set; }
        public static bool Start2Called { get; set; }
        public static bool CallMeAfterAppStartCalled { get; set; }
        public static bool CallMeWhenAppEndsCalled { get; set; }

        internal static void Start()
        {
            if (StartCalled)
            {
                throw new Exception("Unexpected second call to Start");
            }

            StartCalled = true;
            ExecutionLogger.ExecutedOrder += "Start";
        }

        public static void Start2()
        {
            if (Start2Called)
            {
                throw new Exception("Unexpected second call to Start2");
            }

            Start2Called = true;
            ExecutionLogger.ExecutedOrder += "Start2";
        }

        public static void Start3()
        {
            ExecutionLogger.ExecutedOrder += "Start3";
        }

        public static void CallMeAfterAppStart()
        {
            // This gets called after global.asax's Application_Start

            if (CallMeAfterAppStartCalled)
            {
                throw new Exception("Unexpected second call to CallMeAfterAppStart");
            }

            CallMeAfterAppStartCalled = true;
            ExecutionLogger.ExecutedOrder += "CallMeAfterAppStart";
        }

        public static void CallMeWhenAppEnds()
        {
            // This gets called when the app shuts down

            if (CallMeWhenAppEndsCalled)
            {
                throw new Exception("Unexpected second call to CallMeWhenAppEnds");
            }

            CallMeWhenAppEndsCalled = true;
            ExecutionLogger.ExecutedOrder += "CallMeWhenAppEnds";
        }
    }
}
