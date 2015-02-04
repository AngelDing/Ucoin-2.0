using System;
using Microsoft.Practices.Unity;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Ucoin.ServiceModel.Client.Configuration;
using Ucoin.ServiceModel.Core;

namespace Ucoin.ServiceModel.Client
{
    public class ServiceFactory : IServiceFactory
    {       
        public T GetService<T>() where T : class
        {
            return GetService<T>(string.Empty);
        }

        public T GetService<T>(string name) where T : class
        {
            ServiceScope.Current = new ServiceScope();
            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }
            T obj = GetChannel<T>(name);
            ServiceScope.Current.ServiceInfo = obj;
            return (T)obj;
        }

        private bool Validate(IClientChannel channel)
        {
            if (channel == null)
            {
                return false;
            }
            try
            {
                if (channel.State > CommunicationState.Opened)
                {
                    return false;
                }
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            return true;
        }

        private T GetChannel<T>(string name) where T : class
        {
            T obj;

            var clientFactory = ClientFactory.Current;
            if (ClientConfigHelper.IsEndPointExist(name))
            {
                obj = clientFactory.CreateClient<T>(name);
            }
            else
            {
                var config = ClientConfigHelper.GetConfig<T>();
                var address = config.GetAddress<T>();

                if (string.IsNullOrWhiteSpace(address))
                {
                    var msg = string.Format("没有找到EndPoint '{0}'对应的配置," 
                        + "请确认EndPoint是否已经正确配置.", typeof(T).FullName);
                    throw new ArgumentNullException(msg);
                }
                var binding = DefaultEndpointPolicy.EndpointPolicy.DefaultBinding; ;

                obj = clientFactory.CreateClient<T>(binding, new EndpointAddress(address));
            }            
            return obj;
        }
    }
}
