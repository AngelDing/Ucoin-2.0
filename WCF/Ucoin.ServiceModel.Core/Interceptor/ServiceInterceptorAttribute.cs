using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Ucoin.ServiceModel.Core
{
    /// <summary>
    /// 自定义服务扩展
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ServiceInterceptorAttribute : Attribute, IServiceBehavior
    {       
        /// <summary>
        /// 用于更改运行时属性值或插入自定义扩展对象
        /// </summary>
        /// <param name="serviceDescription">服务说明</param>
        /// <param name="host">当前正在生成的宿主</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase host)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                endpoint.Behaviors.Add(new EndpointInterceptor());
            }
        }

        /// <summary>
        /// 用于向绑定元素传递自定义数据，以支持协定实现
        /// </summary>
        /// <param name="serviceDescription">服务的服务说明</param>
        /// <param name="serviceHostBase">服务的宿主</param>
        /// <param name="endpoints">服务的终结点</param>
        /// <param name="bindingParameters">绑定元素可访问的自定义对象</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase
            , Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// 用于检查服务宿主和服务说明，从而确定服务是否可成功运行
        /// </summary>
        /// <param name="serviceDescription">服务的服务说明</param>
        /// <param name="serviceHostBase">服务的宿主</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}
