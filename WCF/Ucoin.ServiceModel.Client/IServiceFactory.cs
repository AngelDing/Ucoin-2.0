using System;

namespace Ucoin.ServiceModel.Client
{
    /// <summary>
    /// 服务构建工厂接口
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// 获取一个服务的实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// 根据配置中对应的名称获取服务的实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="name">服务名称</param>
        /// <returns>服务实例</returns>
        T GetService<T>(string name) where T : class;
    }
}
