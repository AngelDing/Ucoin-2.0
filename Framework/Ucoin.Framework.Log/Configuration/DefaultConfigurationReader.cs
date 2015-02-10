using System.Configuration;

namespace Ucoin.Framework.Logging.Configuration
{
    public class DefaultConfigurationReader : IConfigurationReader
    {
        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }
    }
}
