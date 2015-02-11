using System;
using System.Collections.Specialized;
using Ucoin.Framework.Logging.Configuration;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.Logging.Simple
{
    public abstract class BaseSimpleLoggerAdapter : BaseLoggerAdapter
    {
        public LogArgumentEntity ArgumentEntity { get; set; }

        protected BaseSimpleLoggerAdapter(NameValueCollection properties) :
            this(InitLogArgumentEntity(properties))
        {
        }

        private static LogArgumentEntity InitLogArgumentEntity(NameValueCollection properties)
        {
            var argEntity = new LogArgumentEntity();
            if (properties != null)
            {
                argEntity.Level = properties.Get("level").ToEnum(LogLevel.All);
                argEntity.ShowDateTime = properties.Get("showDateTime").ToBool(true);
                argEntity.ShowLogName = properties.Get("showLogName").ToBool(true);
                argEntity.ShowLevel = properties.Get("showLevel").ToBool(true);
                argEntity.DateTimeFormat = properties.Get("dateTimeFormat");
            }
            return argEntity;
        }

        protected BaseSimpleLoggerAdapter(LogArgumentEntity argEntity)
            : base()
        {
            ArgumentEntity = argEntity;
        }

        protected override ILogger CreateLogger(string name)
        {
            ArgumentEntity.LogName = name;
            return CreateLogger(ArgumentEntity);
        }

        protected abstract ILogger CreateLogger(LogArgumentEntity argEntity);
    }
}
