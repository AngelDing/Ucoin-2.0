using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace Ucoin.ServiceModel.Server.Runtime
{
    internal static class RuntimeUnity
    {
        private static readonly Type _serviceAttributeType = typeof (ServiceContractAttribute);

        private static readonly Lazy<Dictionary<string, string>> _lazy =
            new Lazy<Dictionary<string, string>>(GetServiceConfigInfo);

        public static List<ServicePackage> GetServicePackages()
        {
            var packages = new List<ServicePackage>();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var baseAddress = Unity.GetBasiceAddress();
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
            {
                return packages;
            }

            var q = dir.GetFiles("*.service.dll")
                .Select(c => new ServicePackage(c.FullName, baseAddress));
            packages.AddRange(q);
            foreach (var item in dir.GetDirectories())
            {
                if (item.Name.StartsWith("_"))
                {
                    continue;
                }
                LoadServicePackage(item, packages, baseAddress);
            }
            return packages;
        }

        public static bool ServiceHasConfig(Type serviceType)
        {
            return _lazy.Value.ContainsKey(serviceType.FullName);
        }

        private static Dictionary<string, string> GetServiceConfigInfo()
        {
            var serviceConfig = ConfigurationManager
                .GetSection("system.serviceModel/services") as ServicesSection;
            var dict = new Dictionary<string, string>();
            if (serviceConfig != null)
            {
                foreach (ServiceElement item in serviceConfig.Services)
                {
                    dict.Add(item.Name, string.Empty);
                }
            }
            return dict;
        }

        private static void LoadServicePackage(DirectoryInfo dir,
            List<ServicePackage> packages, string baseAddress)
        {
            var q = dir.GetFiles("*.service.dll")
                .Select(c => new ServicePackage(c.FullName, baseAddress));
            packages.AddRange(q);
            foreach (var item in dir.GetDirectories())
            {
                LoadServicePackage(item, packages, baseAddress);
            }
        }

        public static bool ValidateServiceInterface(Type type)
        {
            if (!type.IsInterface || !type.IsPublic)
            {
                return false;
            }

            return type.GetCustomAttributes(_serviceAttributeType, false).Length > 0;
        }

        public static void ApplyEndpoints(this ServiceHost host, IService service)
        {
            foreach (var item in service.ServiceEndpointes)
            {
                host.AddServiceEndpoint(item);
            }
        }
    }
}
