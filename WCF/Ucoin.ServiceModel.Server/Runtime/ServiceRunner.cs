using System;
using System.Threading;

namespace Ucoin.ServiceModel.Server.Runtime
{
    internal class ServiceRunner : IServiceRunner
    {
        private IServiceRunner _inner;
        private AppDomain _domain;
        private ServicePackage _servicePackage;
        private bool _unloaded;

        public ServiceRunner(ServicePackage package)
        {
            _servicePackage = package;
        }

        public void Load()
        {
            Load(_servicePackage);
        }

        public void Load(ServicePackage package)
        {
            Unload();
            _servicePackage = package;
            var domain = _domain = DomainManager.CreateDomain(package);
            var agent = DomainAgent.CreateInstance(domain);
            _inner = agent.CreateRunner();
            _inner.Load(package);
            _unloaded = false;
        }

        public void Run(IEventListener listener)
        {
            if (_domain == null)
            {
                Load();
            }
            var thread = new Thread(() => { _inner.Run(listener); });
            thread.Start();
        }

        private void Unload()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
            }
            _domain = null;
            _unloaded = true;
        }

        public bool Unloaded
        {
            get { return _unloaded; }
        }
    }
}
