using System;

namespace Ucoin.Framework.Logging.Configuration
{
    public class ConfigurationException : ApplicationException
    {
        public ConfigurationException()
        {
        }

        /// </param>
        public ConfigurationException(string message)
            : base(message)
        {
        }

        /// </param>
        public ConfigurationException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        } 
    }
}
