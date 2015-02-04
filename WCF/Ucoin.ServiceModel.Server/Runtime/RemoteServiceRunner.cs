using System;
using System.Linq;
using System.Security.Permissions;
using System.Reflection;
using Ucoin.ServiceModel.Server.Configuration;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public class RemoteServiceRunner : MarshalByRefObject, IServiceRunner
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        private IService[] services;
        private Assembly _serviceAssembly;

        public void Load(ServicePackage package)
        {
            services = null;
            var address = WcfServerSection.Current.Service.Address;
            if (string.IsNullOrEmpty(address))
            {
                package.BaseAddress = address;
            }
            _serviceAssembly = Assembly.LoadFile(package.AssemblyFile);

            var q = from c in _serviceAssembly.GetTypes()
                let a = c.GetInterfaces().Where(RuntimeUnity.ValidateServiceInterface).ToArray()
                where a.Length > 0
                select new Service(package.Id, c, a, package.BaseAddress);

            services = q.ToArray();
        }

        public void Run(IEventListener listener)
        {
            services.Where(c => c.RunState != RunState.Runnable)
                .ForEach(
                    r =>
                    {
                        try
                        {
                            r.Run();
                            listener.Started(r.Info);
                        }
                        catch (Exception ex)
                        {
                            listener.Error(r.Info, ex);
                        }
                    }
                );
        }
    }
}
