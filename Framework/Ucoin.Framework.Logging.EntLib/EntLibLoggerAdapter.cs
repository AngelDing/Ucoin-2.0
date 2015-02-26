using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections.Specialized;
using Ucoin.Framework.Utility;

namespace Ucoin.Framework.Logging.EntLib
{
    public class EntLibLoggerAdapter : BaseLoggerAdapter
    {
        //private readonly LoggerSettings settings;
        private LogWriter logWriter;

        //public int DefaultPriority
        //{
        //    get { return settings.LogPriority; }
        //}

        //public string ExceptionFormat
        //{
        //    get { return settings.ExceptionFormat; }
        //}

        public LogWriter LogWriter
        {
            get
            {
                if (logWriter == null)
                {
                    lock (this)
                    {
                        if (logWriter == null)
                        {
                            logWriter = Logger.Writer;
                        }
                    }
                }
                return logWriter;
            }
        }


        public EntLibLoggerAdapter()
            : this(null)
        {
        }

        public EntLibLoggerAdapter(LogWriter writer)
            : base()
        {
            //if (string.IsNullOrEmpty(exceptionFormat))
            //{
            //    exceptionFormat = LoggerSettings.DEFAULTEXCEPTIONFORMAT;
            //}
            //settings = new LoggerSettings(defaultPriority, exceptionFormat);
            logWriter = writer;
        }

        public EntLibLoggerAdapter(NameValueCollection properties)
            : base()
        {
            //if (properties == null)
            //{
            //    settings = new LoggerSettings(
            //        LoggerSettings.DEFAULTPRIORITY,
            //        LoggerSettings.DEFAULTEXCEPTIONFORMAT);
            //}
            //else
            //{
            //    var priority = properties.Get("priority").ToInt(LoggerSettings.DEFAULTPRIORITY);
            //    var format = properties.Get("exceptionFormat");
            //    if (string.IsNullOrEmpty(format))
            //    {
            //        format = LoggerSettings.DEFAULTEXCEPTIONFORMAT;
            //    }
            //    settings = new LoggerSettings(priority, format);
            //}
        }

        protected override ILogger CreateLogger(string name)
        {
            return CreateLogger(name, LogWriter);
        }

        protected virtual ILogger CreateLogger(string name, LogWriter logWriter)
        {
            return new EntLibLogger(name, LogWriter);
        }
    }
}