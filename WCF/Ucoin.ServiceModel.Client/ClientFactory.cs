using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Collections.Concurrent;
using System.ServiceModel.Configuration;
using System.Configuration;
//using ProtoBuf.ServiceModel;
using Ucoin.ServiceModel.Core;

namespace Ucoin.ServiceModel.Client
{
    /// <summary>
    /// WCF Client 上下文管理
    /// </summary>
    public sealed class ClientFactory : IDisposable
    {
        private static ClientFactory _current;
        private readonly ConcurrentDictionary<int, object> _channelFactories = 
            new ConcurrentDictionary<int, object>();

        private ClientFactory()
        {
            _current = this;
        }

        /// <summary>
        /// 获取或者创建一个通道，该通道用于将消息发送到以指定方式配置其终结点的服务
        /// </summary>
        /// <typeparam name="T">通道类型</typeparam>
        /// <returns>工厂创建的 IChannel 类型的 TChannel</returns>
        public T CreateClient<T>()
        {
            var factory = new ChannelFactory<T>();
            var result = CreateChannel(factory);
            return (T) result;
        }

        /// <summary>
        /// 获取或者创建一个通道，该通道用于将消息发送到以指定方式配置其终结点的服务
        /// </summary>
        /// <typeparam name="T">通道类型</typeparam>
        /// <param name="binding">为由工厂生成的通道指定的 <see cref="System.ServiceModel.Channels.Binding"/></param>
        /// <param name="remoteAddress">提供服务位置的 <see cref="EndpointAddress"/></param>
        /// <returns>工厂创建的 IChannel 类型的 TChannel</returns>
        public T CreateClient<T>(Binding binding, EndpointAddress remoteAddress)
        {
            var factory = GetFacotry<T>(binding, remoteAddress);
            var result = CreateChannel(factory);

            return (T) result;
        }

        /// <summary>
        /// 获取或者创建一个通道，该通道用于将消息发送到以指定方式配置其终结点的服务
        /// </summary>
        /// <typeparam name="T">通道类型</typeparam>
        /// <param name="endpointConfigurationName">用于终结点的配置名称</param>
        /// <param name="remoteAddress">提供服务位置的 <see cref="EndpointAddress"/></param>
        /// <returns>工厂创建的 IChannel 类型的 TChannel</returns>
        private T CreateClient<T>(string endpointConfigurationName, EndpointAddress remoteAddress)
        {
            var factory = GetFacotry<T>(endpointConfigurationName, remoteAddress);
            var result = CreateChannel(factory);
            return (T) result;
        }

        /// <summary>
        /// 获取或者创建一个通道，该通道用于将消息发送到以指定方式配置其终结点的服务
        /// </summary>
        /// <typeparam name="T">通道类型</typeparam>
        /// <param name="endpointConfigurationName">用于终结点的配置名称</param>
        /// <returns>工厂创建的 IChannel 类型的 TChannel</returns>
        public T CreateClient<T>(string endpointConfigurationName)
        {
            return CreateClient<T>(endpointConfigurationName, null);
        }

        private ChannelFactory<T> GetFacotry<T>(Binding binding, EndpointAddress remoteAddress)
        {
            var key = (typeof (T).FullName + "," + binding.GetType().Name
                       + "," + (remoteAddress != null ? remoteAddress.Uri.ToString() : string.Empty)).GetHashCode();

            object val;
            if (_channelFactories.TryGetValue(key, out val))
            {
                return val as ChannelFactory<T>;
            }
            var factory = SetupCustomerBehaviors(new ChannelFactory<T>(binding, remoteAddress));
            _channelFactories.TryAdd(key, factory);
            return factory;
        }

        private ChannelFactory<T> GetFacotry<T>(string endpointConfigurationName, EndpointAddress remoteAddress)
        {
            var key = (typeof (T).FullName + "," + endpointConfigurationName
                       + "," + (remoteAddress != null ? remoteAddress.Uri.ToString() : string.Empty)).GetHashCode();
            object val;
            if (_channelFactories.TryGetValue(key, out val))
            {
                return val as ChannelFactory<T>;
            }

            var cFactory = new ChannelFactory<T>(endpointConfigurationName, remoteAddress);
            var factory = SetupCustomerBehaviors(cFactory);
            _channelFactories.TryAdd(key, factory);
            return factory;
        }

        private ChannelFactory<T> SetupCustomerBehaviors<T>(ChannelFactory<T> factory)
        {
            var endpoint = factory.Endpoint;
            //endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            endpoint.Behaviors.Add(new EndpointInterceptor());
            ApplyDefaultBehaviors(factory);
            return factory;
        }

        private void ApplyDefaultBehaviors(ChannelFactory factory)
        {
            var section = (BehaviorsSection)ConfigurationManager
                .GetSection("system.serviceModel/behaviors");
            foreach (EndpointBehaviorElement item in section.EndpointBehaviors)
            {
                if (!string.IsNullOrEmpty(item.Name))
                    continue;
                foreach (var be in item)
                {
                    if (be.BehaviorType != null && factory.Endpoint.Behaviors.Contains(be.BehaviorType))
                    {
                        continue;
                    }
                    if (be.BehaviorType != null)
                    {
                        var obj = Activator.CreateInstance(be.BehaviorType) as IEndpointBehavior;
                        factory.Endpoint.Behaviors.Add(obj);
                    }
                }
            }
        }

        private IClientChannel CreateChannel<T>(ChannelFactory<T> factory)
        {
            return factory.CreateChannel() as IClientChannel;
        }

        public static ClientFactory Current
        {
            get { return _current ?? (_current = new ClientFactory()); }
        }

        public void Dispose()
        {
            if (_current == this)
            {
                _current = null;
            }
        }
    }
}