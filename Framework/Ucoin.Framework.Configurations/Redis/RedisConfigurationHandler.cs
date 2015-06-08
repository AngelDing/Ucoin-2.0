using System;
using System.Configuration;
using Ucoin.Framework.Extensions;
using Ucoin.Framework.Utils;

namespace Ucoin.Framework.Configurations
{
    public class RedisConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RedisHostGroupCollection HostGroups
        {
            get
            {
                return this[""] as RedisHostGroupCollection;
            }
        }

        public static IRedisConfiguration GetConfig(AppCodeType? appCodeType = null)
        {
            var configName = string.Empty;
            if (appCodeType == null)
            {
                var codeType = ConfigurationManager.AppSettings["AppCodeType"];
                GuardHelper.ArgumentNotNull(() => codeType);
                configName = codeType.ToString();
            }
            else
            {
                configName = appCodeType.Value.GetDescription();
            }
           
            var handler = ConfigurationManager.GetSection("redisConfig") as RedisConfigurationHandler;
            GuardHelper.ArgumentNotNull(() => handler);
            RedisHostGroup group = null;

            foreach (RedisHostGroup item in handler.HostGroups)
            {
                if (item.Name.Equals(configName, StringComparison.OrdinalIgnoreCase))
                {
                    group = item;
                    break;
                }
            }

            if (group == null)
            {
                throw new Exception(string.Format("Redis配置错误，根據服務器配置組名:{0}找不到Redis服務器组", configName));
            }

            return group;
        }
    }
}
