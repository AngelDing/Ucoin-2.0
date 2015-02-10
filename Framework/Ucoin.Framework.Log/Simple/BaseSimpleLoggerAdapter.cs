using System;
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
            var argEntity = new LogArgumentEntity
            {
                Level = properties.GetValue("level").ToEnum(LogLevel.All),
                ShowDateTime = properties.GetValue("showDateTime").ToBool(true),
                ShowLogName = properties.GetValue("showLogName").ToBool(true),
                ShowLevel = properties.GetValue("showLevel").ToBool(true),
                DateTimeFormat = properties.GetValue("dateTimeFormat", string.Empty)
            };
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
