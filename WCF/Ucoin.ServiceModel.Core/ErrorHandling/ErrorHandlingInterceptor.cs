using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;

namespace Ucoin.ServiceModel.Core
{
    /// <summary>
    /// 定义个ErrorHandling
    /// </summary>
    public sealed class ErrorHandlingInterceptor : IServiceBehavior
    {
        private readonly string _policyName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="policyName"></param>
        public ErrorHandlingInterceptor(string policyName)
        {
            _policyName = policyName;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase chanDispBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = chanDispBase as ChannelDispatcher;
                if (channelDispatcher == null)
                {
                    continue;
                }
                channelDispatcher.ErrorHandlers.Add(new ErrorHandler(_policyName));
            }
        }
    }
}
