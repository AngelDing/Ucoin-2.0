using System;
using System.ServiceModel;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public sealed class ServiceHostProxy : ServiceHost
    {
        public ServiceHostProxy(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        public ServiceHostProxy(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
        }
    }
}