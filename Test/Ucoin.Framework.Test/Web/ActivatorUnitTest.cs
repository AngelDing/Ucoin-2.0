using Ucoin.Framework.Test.Web.Library;
using Ucoin.Framework.Web.Activator;
using Xunit;

namespace Ucoin.Framework.Test.Web
{
    public class ActivatorUnitTest
    {
        public ActivatorUnitTest()
        {
            ActivationManager.Reset();
            ExecutionLogger.ExecutedOrder = "";
            MyStartupCode.StartCalled = false;
            MyStartupCode.Start2Called = false;
            MyStartupCode.CallMeAfterAppStartCalled = false;
            MyStartupCode.CallMeWhenAppEndsCalled = false;

            MyOtherStartupCode.StartCalled = false;
            MyOtherStartupCode.Start2Called = false;
        }

        [Fact]
        public void TestWebActivatorAllStartMethodsGetCalled()
        {
            ActivationManager.Run();

            Assert.True(MyStartupCode.StartCalled);
            Assert.True(MyStartupCode.Start2Called);
            Assert.True(MyStartupCode.CallMeAfterAppStartCalled);
        }

        [Fact]
        public void TestWebActivatorPreStartMethodsGetCalled()
        {
            ActivationManager.RunPreStartMethods();

            Assert.True(MyStartupCode.StartCalled);
            Assert.True(MyStartupCode.Start2Called);
            Assert.False(MyStartupCode.CallMeAfterAppStartCalled);
        }

        [Fact]
        public void TestWebActivatorPreStartMethodsInDesignerModeGetCalled()
        {
            ActivationManager.RunPreStartMethods(designerMode: true);

            Assert.True(MyStartupCode.StartCalled);
            Assert.False(MyStartupCode.Start2Called);
            Assert.False(MyStartupCode.CallMeAfterAppStartCalled);
        }

        [Fact]
        public void TestWebActivatorPostStartMethodsGetCalled()
        {
            ActivationManager.RunPostStartMethods();

            Assert.False(MyStartupCode.StartCalled);
            Assert.False(MyStartupCode.Start2Called);
            Assert.True(MyStartupCode.CallMeAfterAppStartCalled);
        }

        [Fact]
        public void TestWebActivatorShutdownMethodsGetCalled()
        {
            ActivationManager.RunShutdownMethods();

            Assert.True(MyStartupCode.CallMeWhenAppEndsCalled);
        }

        [Fact]
        public void TestWebActivatorMethodsCalledBySpecifiedOrder()
        {
            ActivationManager.Run();
            ActivationManager.RunShutdownMethods();
            Assert.Equal("StartStart3OtherStartStart2OtherStart2CallMeAfterAppStartCallMeWhenAppEnds", ExecutionLogger.ExecutedOrder);
        }
    }
}
