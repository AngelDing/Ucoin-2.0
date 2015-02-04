using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ucoin.ServiceModel.Client.Configuration;

namespace Ucoin.ServiceModel.Client
{
    /// <summary>
    /// 客戶端服务代理
    /// </summary>
    public class ServiceProxy
    {
        private static IServiceFactory GetInstance<T>()
        {
            return Container.Current.Resolve<IServiceFactory>();
        }

        static ServiceProxy()
        {
            InitialFactories();
        }

        private static void InitialFactories()
        {
            var policy = new InterceptionBehavior<PolicyInjectionBehavior>();
            var intercptor = new Interceptor<TransparentProxyInterceptor>();
            var type = typeof(ServiceFactory);
            Container.Current.RegisterType(
                typeof(IServiceFactory),
                type,
                new ContainerControlledLifetimeManager(),
                policy,
                intercptor
            );
        }

        /// <summary>
        /// 获取类型T实例,如无特殊配置，建议优先调用此方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>T 的实例</returns>
        public static T GetService<T>() where T : class
        {
            return GetService<T>(null);
        }

        /// <summary>
        /// 根据服务在容器中的配置名称从服务容器中获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="name">服务的名称</param>
        /// <returns>T的实例</returns>
        public static T GetService<T>(string name) where T : class
        {
            return GetInstance<T>().GetService<T>(name);
        }       
    }
}
