using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Ucoin.ServiceModel.Core
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