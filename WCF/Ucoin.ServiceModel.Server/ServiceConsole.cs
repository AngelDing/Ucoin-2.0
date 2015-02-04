using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections.Concurrent;
using Ucoin.ServiceModel.Server.Runtime;

namespace Ucoin.ServiceModel.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class ServiceConsole 
    {
        private static readonly Lazy<List<ServicePackage>> _packages = 
            new Lazy<List<ServicePackage>>(LoadPackages);

        private static readonly ConcurrentDictionary<int, ServiceRunner> _serviceRuners =
            new ConcurrentDictionary<int, ServiceRunner>();

        private static readonly object _sync = new object();
        private static readonly IEventListener _lisener = new EventListener();

        internal void StartServices(IEventListener listener, Action<Exception> error = null)
        {
            var packages = _packages.Value;
            foreach (var p in packages)
            {
                try
                {
                    var el = new EventListener();
                    el.Notify += (o, e) => { if (e.Type == "error") error(e.Exception); };

                    StartService(p, listener ?? el);
                }
                catch (CommunicationException ce)
                {
                    throw new NotSupportedException(
                        "请确认Net.Tcp Port Sharing Service等服务是否开启，以及服务器配置是否正确。", ce);
                }
                catch (Exception ex)
                {
                    if (error != null)
                    {
                        error(ex);
                    }
                }
            }
        }

        private void StartService(ServicePackage package, IEventListener listener)
        {
            var runner = GetServiceRuner(package);
            if (runner.Unloaded)
            {
                runner.Load();
            }
            if (listener == null)
            {
                listener = GetEventListener();
            }
            runner.Run(listener);
        }

        private static List<ServicePackage> LoadPackages()
        {
            var values = RuntimeUnity.GetServicePackages();
            return values;
        }

        private ServiceRunner GetServiceRuner(ServicePackage package)
        {
            ServiceRunner runner;
            if (!_serviceRuners.TryGetValue(package.Id, out runner))
            {
                lock (_sync)
                {
                    runner = CreateServiceRuner(package);
                    _serviceRuners.TryAdd(package.Id, runner);
                }
            }
            return runner;
        }

        private ServiceRunner CreateServiceRuner(ServicePackage package)
        {
            var runer = new ServiceRunner(package);
            runer.Load();
            return runer;
        }

        private IEventListener GetEventListener()
        {
            return _lisener;
        }
    }
}
