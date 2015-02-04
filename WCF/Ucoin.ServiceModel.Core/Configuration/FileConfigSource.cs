using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;

namespace Ucoin.ServiceModel.Core.Configuration
{
    [ConfigurationElementType(typeof(FileConfigElement))]
    public class FileConfigSource : FileConfigurationSource
    {
        public FileConfigSource(string configurationFilepath)
            : this(configurationFilepath, true)
        {

        }

        private FileConfigSource(string configurationFilepath, bool referesh)
            : this(configurationFilepath, referesh, 15000)
        {

        }

        private FileConfigSource(string configurationFilepath, bool referesh, int refreshInterval) :
            base(RetriveActualFilePath(configurationFilepath), referesh, refreshInterval)
        {

        }

        static string RetriveActualFilePath(string file)
        {
            if (!Directory.Exists(file))
            {
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            return file;
        }
    }

    public class FileConfigElement : FileConfigurationSourceElement
    {
        public override IConfigurationSource CreateSource()
        {
            IConfigurationSource createdObject = new FileConfigSource(FilePath);

            return createdObject;
        }
    }
}
