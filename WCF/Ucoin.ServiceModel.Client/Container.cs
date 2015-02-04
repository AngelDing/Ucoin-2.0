using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Ucoin.ServiceModel.Client
{
    /// <summary>
    /// IOC 容器
    /// </summary>
    public static class Container
    {
        private const string CONTAINER_NAME = "Ucoin";
        private static readonly Lazy<IUnityContainer> _current;

        static Container()
        {
            _current = new Lazy<IUnityContainer>(Create);
        }

        private static IUnityContainer Create()
        {
            var current = new UnityContainer();
            try
            {
                current.LoadConfiguration(CONTAINER_NAME);
            }
            catch (ConfigurationErrorsException ce)
            {
                throw new ConfigurationErrorsException("Unity配置错误，请确认配置文件中存在Unity配置节", ce);
            }

            return current;
        }

        /// <summary>
        /// Unity容器
        /// </summary>
        public static IUnityContainer Current
        {
            get { return _current.Value; }
        }
    }
}
