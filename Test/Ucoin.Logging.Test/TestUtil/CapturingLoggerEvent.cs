using System;
using Ucoin.Framework.Logging;

namespace Ucoin.Logging.Test
{
    public class CapturingLoggerEvent
    {
        public readonly CapturingLogger Source;

        public readonly LogLevel Level;

        public readonly object MessageObject;

        public readonly Exception Exception;

        public string RenderedMessage
        {
            get { return MessageObject.ToString(); }
        }

        public CapturingLoggerEvent(CapturingLogger source, LogLevel level, object msg, Exception ex)
        {
            Source = source;
            Level = level;
            MessageObject = msg;
            Exception = ex;
        }
    }
}
