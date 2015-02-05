using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace CtripSZ.ServiceModel.Server.Interceptor
{
    internal class CallContextInitializer : ICallContextInitializer
    {
        public void AfterInvoke(object correlationState)
        {
        }

        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {
            return null;
        }
    }
}