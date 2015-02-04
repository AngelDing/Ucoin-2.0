using System;
using System.IO;
using System.Security.Policy;
using System.Reflection;
using System.Security;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public class DomainManager
    {
        public static AppDomain CreateDomain(ServicePackage package)
        {
            var setup = new AppDomainSetup();
            setup.ApplicationName = "Service" + "_" + Environment.TickCount;
            var serviceFile = new FileInfo(package.FullName);

            var appBase = serviceFile.DirectoryName;
            var configFile = GetConfigFile(package, serviceFile);

            setup.ApplicationBase = appBase;
            setup.ConfigurationFile = appBase != null && configFile != null
                ? Path.Combine(appBase, configFile)
                : configFile;
            var binPath = appBase;
            setup.PrivateBinPath = binPath;
            var domainName = "service-domain-" + package.Name;

            var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            if (evidence.GetAssemblyEnumerator().Current == null)
            {
                var zone = new Zone(SecurityZone.MyComputer);
                evidence.AddHostEvidence(zone);
                var assembly = Assembly.GetExecutingAssembly();
                var url = new Url(assembly.CodeBase);
                evidence.AddHostEvidence(url);
                var hash = new Hash(assembly);
                evidence.AddHostEvidence(hash);
            }

            return AppDomain.CreateDomain(domainName, evidence, setup);
        }

        private static string GetConfigFile(ServicePackage package, FileInfo serviceFile)
        {
            string configFile;
            if (serviceFile.DirectoryName != null)
            {
                configFile = Path.Combine(serviceFile.DirectoryName, "app.config");
                if (File.Exists(configFile))
                {
                    return configFile;
                }
                configFile = Path.Combine(serviceFile.DirectoryName,
                    AppDomain.CurrentDomain.FriendlyName) + ".config";
                if (File.Exists(configFile))
                {
                    return configFile;
                }
            }
            throw new ArgumentNullException("没有找到对应服务的配置文件：" + package.FullName);
        }
    }
}