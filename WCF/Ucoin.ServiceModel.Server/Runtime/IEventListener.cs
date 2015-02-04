using System;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public interface IEventListener
    {
        void Started(ServiceInfo service);

        void Stoped(ServiceInfo service);

        void Error(ServiceInfo service, Exception ex);
    }
}
