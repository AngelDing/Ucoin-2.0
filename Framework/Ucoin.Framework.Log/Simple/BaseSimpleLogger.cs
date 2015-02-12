using System;
using System.Globalization;
using System.Text;
 
namespace Ucoin.Framework.Logging.Simple
{
    public abstract class BaseSimpleLogger : BaseLogger
    {
        private readonly LogArgumentEntity argumentEntity;

        public LogArgumentEntity ArgumentEntity
        {
            get { return argumentEntity; }
        }        

        public BaseSimpleLogger(LogArgumentEntity argEntity)
        {
            argumentEntity = argEntity;
        }

        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevel level, object message, Exception e)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException("stringBuilder");
            }
            if (ArgumentEntity.ShowDateTime)
            {
                if (ArgumentEntity.HasDateTimeFormat)
                {
                    stringBuilder.Append(DateTime.Now.ToString(ArgumentEntity.DateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    stringBuilder.Append(DateTime.Now);
                }

                stringBuilder.Append(" ");
            }

            if (ArgumentEntity.ShowLevel)
            {
                stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            }

            if (ArgumentEntity.ShowLogName)
            {
                stringBuilder.Append(ArgumentEntity.LogName).Append(" - ");
            }

            stringBuilder.Append(message);

            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(ExceptionFormatter.Format(e));
            }
        }

        protected virtual bool IsLevelEnabled(LogLevel level)
        {
            int iLevel = (int)level;
            int iCurrentLogLevel = (int)ArgumentEntity.Level;
            return (iLevel >= iCurrentLogLevel);
        }

        #region ILogger Members

        public override bool IsTraceEnabled
        {
            get { return IsLevelEnabled(LogLevel.Trace); }
        }

        public override bool IsDebugEnabled
        {
            get { return IsLevelEnabled(LogLevel.Debug); }
        }

        public override bool IsInfoEnabled
        {
            get { return IsLevelEnabled(LogLevel.Info); }
        }

        public override bool IsWarnEnabled
        {
            get { return IsLevelEnabled(LogLevel.Warn); }
        }

        public override bool IsErrorEnabled
        {
            get { return IsLevelEnabled(LogLevel.Error); }
        }

        public override bool IsFatalEnabled
        {
            get { return IsLevelEnabled(LogLevel.Fatal); }
        }

        #endregion
    }
}
