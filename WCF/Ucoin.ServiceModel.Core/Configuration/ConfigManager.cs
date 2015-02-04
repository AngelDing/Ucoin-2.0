using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Ucoin.ServiceModel.Core.Configuration
{
    /// <summary>
    /// 配置文件管理
    /// </summary>
    public class ConfigManager : IDisposable
    {
        private readonly IConfigurationSource _config;
        private static readonly Lazy<ConfigManager> _lazy = 
            new Lazy<ConfigManager>(() => new ConfigManager());

        private ConfigManager()
        {
            _config = ConfigurationSourceFactory.Create();
        }

        private ConfigManager(string fileName, bool refresh)
        {
            _config = new FileConfigurationSource(fileName, refresh);
        }

        /// <summary>
        /// 获取一个配置节
        /// </summary>
        /// <typeparam name="T">配置节类型</typeparam>
        /// <param name="sectionName">配置节名称</param>
        /// <returns>T类型的配置节实例</returns>
        public static T GetSection<T>(string sectionName) where T : ConfigurationElement
        {
            var section = (T)_lazy.Value.GetSection(sectionName);
            return section;
        }

        /// <summary>
        /// 根据配置文件获取对应的配置管理
        /// </summary>
        /// <param name="fileName">配置文件全名称</param>
        /// <returns>配置管理</returns>
        public static ConfigManager Create(string fileName)
        {
            return new ConfigManager(fileName, false);
        }

        /// <summary>
        /// 获取配置节
        /// </summary>
        /// <param name="sectionName">配置节名称</param>
        /// <returns>配置节</returns>
        public object GetSection(string sectionName)
        {
            return _config.GetSection(sectionName);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _config.Dispose();
        }
    }
}
