using System;
using System.Collections.Generic;
using Ucoin.Framework.Logging;
using Ucoin.Framework.Logging.Simple;
using Ucoin.Framework.Utility;

namespace Ucoin.Logging.Test
{
    public class CapturingLogger : BaseSimpleLogger
    {
        /// <summary>
        /// The adapter that created this logger instance.
        /// </summary>
        public readonly CapturingLoggerAdapter Owner;

        private volatile CapturingLoggerEvent lastEvent;

        public CapturingLoggerEvent LastEvent
        {
            get { return lastEvent; }
        }

        public void Clear()
        {
            lock (LoggerEvents)
            {
                lastEvent = null;
                LoggerEvents.Clear();
            }
        }

        public readonly IList<CapturingLoggerEvent> LoggerEvents = new List<CapturingLoggerEvent>();

        public virtual void AddEvent(CapturingLoggerEvent loggerEvent)
        {
            lastEvent = loggerEvent;
            lock (LoggerEvents)
            {
                LoggerEvents.Add(loggerEvent);
            }
            Owner.AddEvent(LastEvent);
        }

        public CapturingLogger(CapturingLoggerAdapter owner, string logName)
            : base(InitLogArgumentEntity(logName))
        {
            owner.CheckNotNull("owner");
            Owner = owner;
        }

        private static LogArgumentEntity InitLogArgumentEntity(string logName)
        {
            var argEntity = new LogArgumentEntity
            {
                LogName = logName,
                Level = LogLevelType.All,
                ShowDateTime = true,
                ShowLevel = true,
                ShowLogName = true,
                DateTimeFormat = null
            };
            return argEntity;
        }


        protected override void Write(LogLevelType level, object message, Exception exception)
        {
            CapturingLoggerEvent ev = new CapturingLoggerEvent(this, level, message, exception);
            AddEvent(ev);
        }
    }
}
