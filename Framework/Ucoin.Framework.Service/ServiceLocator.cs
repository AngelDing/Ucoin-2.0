using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;

namespace Ucoin.Framework.Service
{
    /// <summary>
    /// Represents the Service Locator.
    /// </summary>
    public sealed class ServiceLocator
    {
        private IUnityContainer container;
        private static readonly ServiceLocator instance = new ServiceLocator();

        /// <summary>
        /// Initializes a new instance of <c>ServiceLocator</c> class.
        /// </summary>
        private ServiceLocator()
        {
            if (container == null)
            {
                var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                if (section == null)
                {
                    return;
                }
                container = new UnityContainer();
                section.Configure(container);
            }
        }

        /// <summary>
        /// 對於某些無配置的程序，支持代碼注入，如：
        ///  var container = ServiceLocator.GetUnityContainer();
        ///  if (container.IsRegistered<IOrderLogService>() == false)
        ///  {
        ///     container.RegisterType<IOrderLogService, OrderLogService>();
        ///  }
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer GetUnityContainer()
        {
            instance.container = new UnityContainer();
            return instance.container;
        }
       
        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public static T GetService<T>()
        {
            try
            {
                return instance.container.Resolve<T>();
            }
            catch (Exception ex)
            {
                var type = typeof(T);
                throw new ActivationException(string.Format("Service not found [{0}]", type.Name), ex);
            }
        }           
    }
}
