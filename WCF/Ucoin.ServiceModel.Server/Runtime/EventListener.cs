using System;
using System.Diagnostics;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [Serializable]
    public class ServiceEventArgs : EventArgs
    {
        public ServiceInfo ServiceInfo { get; set; }
        public Exception Exception { get; set; }
        public string Type { get; set; }
    }

    [Serializable]
    public sealed class EventListener : MarshalByRefObject, IEventListener
    {
        public event EventHandler<ServiceEventArgs> Notify;

        public void Started(ServiceInfo service)
        {
            Trace.TraceInformation("EventListener\t" + service.FullName + " started:");
            foreach (var item in service.EndpointsInfo)
            {
                Trace.TraceInformation("EventListener\t" + item.Address);
            }
            OnNotify(service, null, "started");
        }

        private void OnNotify(ServiceInfo service, Exception ex, string type)
        {
            if (Notify != null)
            {
                Notify(this, new ServiceEventArgs {ServiceInfo = service, Exception = ex, Type = type});
            }
        }

        public void Stoped(ServiceInfo service)
        {
            OnNotify(service, null, "stoped");
        }

        public void Error(ServiceInfo service, Exception ex)
        {
            OnNotify(service, ex, "error");
        }
    }
}
