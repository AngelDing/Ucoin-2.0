using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Ucoin.ServiceModel.Core
{
    /// <summary>
    /// 异常Behavior配置
    /// </summary>
    public class ExceptionHandlingBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// 异常策略名称
        /// </summary>
        [ConfigurationProperty("exceptionPolicy")]
        public string ExceptionPolicy
        {
            get
            { return this["exceptionPolicy"] as string; }
            set
            { this["exceptionPolicy"] = value; }
        }
        
        /// <summary>
        /// 类型
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(ErrorHandlingInterceptor); }
        }

        /// <summary>
        /// 构建Behavior
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new ErrorHandlingInterceptor(ExceptionPolicy);
        }
    }
}


