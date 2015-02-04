using Ucoin.ServiceModel.Core;
//using ProtoBuf.ServiceModel;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [Serializable]
    public class Service : IService
    {
        private readonly ServiceInfo _info;
        private readonly Type _serviceType;
        private readonly Type[] _contracts;
        private ServiceHost _host;
        private ServiceEndpoint[] _addresses;

        public Service(int packageId, Type serviceType, Type[] contracts, string baseAddress = null)
        {
            _info = new ServiceInfo
            {
                FullName = serviceType.FullName,
                Name = serviceType.Name,
                PackageId = packageId,
                BaseAddress = baseAddress
            };
            _serviceType = serviceType;
            var address = Unity.GetBasiceAddress();
            if (!string.IsNullOrEmpty(address))
            {
                _info.BaseAddress = address;
            }
            _contracts = contracts;
        }

        public ServiceInfo Info
        {
            get
            {
                _info.State = RunState;
                return _info;
            }
        }

        public RunState RunState
        {
            get
            {
                if (_info.State == Runtime.RunState.Error)
                {
                    return RunState.Error;
                }
                if (_host == null)
                {
                    return RunState.NotRunnable;
                }
                if (_host.State == CommunicationState.Faulted)
                {
                    return RunState.Error;
                }
                return _host.State == CommunicationState.Opened
                    ? RunState.Runnable 
                    : Runtime.RunState.NotRunnable;
            }
        }

        public void Run()
        {
            if (_host == null)
            {
                var isSingle = IsSingle(_serviceType);
                _host = !isSingle
                    ? new ServiceHostProxy(_serviceType)
                    : new ServiceHostProxy(Activator.CreateInstance(_serviceType));

                if (!RuntimeUnity.ServiceHasConfig(ServiceType))
                {
                    _host.ApplyEndpoints(this);
                }               
                LoadEndpointsInfomation();
            }
            try
            {
                if (_host.State != CommunicationState.Opening)
                {
                    _host.Open();
                }
            }
            catch
            {
                Info.State = Runtime.RunState.Error;
                _host.Abort();
                _host = null;
                throw;
            }
        }

        public ServiceEndpoint[] ServiceEndpointes
        {
            get
            {
                if (_addresses == null)
                {
                    _addresses = Contracts.Select(c => DefaultEndpointPolicy.EndpointPolicy.
                        CreateServiceEndpoint(c, _info.BaseAddress)).ToArray();
                }
                return _addresses;
            }
        }

        private Type ServiceType
        {
            get { return _serviceType; }
        }

        private Type[] Contracts
        {
            get { return _contracts; }
        }

        private void LoadEndpointsInfomation()
        {
            _info.EndpointsInfo = _host.Description.Endpoints.Select(c =>
            {
                var ei = new EndpointInfo();
                ei.ContractName = c.Contract.ContractType.FullName;
                ei.Binding = c.Binding.Name;
                ei.Address = c.Address.Uri.ToString();
                return ei;
            }).ToArray();
        }

        private bool IsSingle(Type type)
        {
            var atrs = type.GetCustomAttributes(typeof (ServiceBehaviorAttribute), false);
            if (atrs.Length == 0)
            {
                return false;
            }
            var si = atrs.FirstOrDefault() as ServiceBehaviorAttribute;
            return si.ConcurrencyMode == ConcurrencyMode.Single;
        }
    }
}
