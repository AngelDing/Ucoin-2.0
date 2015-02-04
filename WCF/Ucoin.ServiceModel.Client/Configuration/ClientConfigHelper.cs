using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Ucoin.ServiceModel.Client.Configuration
{
    public class ClientConfigHelper
    {
        static readonly ConcurrentDictionary<RuntimeTypeHandle, ClientConfigItem> _clientConfigItems =
                new ConcurrentDictionary<RuntimeTypeHandle, ClientConfigItem>();
        static ClientConfigItem defaultConfig;

        #region public
        
        public static ClientConfigItem GetConfig<T>() where T : class
        {
            var t = typeof(T);
            return GetConfig(t);
        }

        private static ClientConfigItem GetConfig(Type contract)
        {
            var t = contract;
            ClientConfigItem item;
            if (_clientConfigItems.TryGetValue(t.TypeHandle, out item))
                return item;

            var assemblyName = t.Assembly.FullName.Split(',')[0].Trim();
            var typeName = t.FullName;
            var q = ConfigItems.FirstOrDefault(c => (c.Type == typeName && c.Assembly == assemblyName));// || );
            if (q != null)
            {
                item = new ClientConfigItem
                {
                    Address = q.Address,
                    Assembly = q.Assembly
                };
            }
            else
            {
                q = ConfigItems.FirstOrDefault(c =>
                    c.Assembly == assemblyName && c.Type == string.Empty);
                if (q != null)
                {
                    item = new ClientConfigItem
                    {
                        BaseAddress = q.Address,
                        Assembly = q.Assembly
                    };
                }
                else
                {
                    var temp = GetDefaultConfig();
                    item = new ClientConfigItem
                    {
                        BaseAddress = temp.BaseAddress,
                        Assembly = temp.Assembly
                    };
                }
            }

            _clientConfigItems.TryAdd(t.TypeHandle, item);

            return item;
        }

        public static bool IsEndPointExist(string configName)
        {
            return ExistsClientSettingName(configName);
        }
        #endregion

        #region private
        
        private static ClientConfigItem GetDefaultConfig()
        {
            if (defaultConfig == null)
            {
                var item = ConfigItems.FirstOrDefault(c => c.Assembly == "*");
                if (item != null)
                    defaultConfig = new ClientConfigItem
                    {
                        BaseAddress = item.Address,
                        Assembly = item.Assembly
                    };
                else
                    defaultConfig = new ClientConfigItem();
            }
            return defaultConfig;
        }

        static bool ExistsClientSettingName(string name)
        {
            var clients = ConfigurationManager
                .GetSection("system.serviceModel/client") as ClientSection;
           
            if (clients == null)
                return false;

            return clients.Endpoints.Cast<ChannelEndpointElement>()
                .FirstOrDefault(c => c.Name == name) != null;
        }

        static List<ClientElement> ConfigItems
        {
            get
            {
                var section = WcfClientConfig.Current.Clients;
                return section.Cast<ClientElement>().OrderByDescending(c => c.Type).ToList();
            }
        }
        #endregion
    }
}
