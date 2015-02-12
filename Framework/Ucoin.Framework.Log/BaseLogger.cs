using System;

namespace Ucoin.Framework.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public abstract bool IsTraceEnabled { get; }

        public abstract bool IsDebugEnabled { get; }

        public abstract bool IsInfoEnabled { get; }

        public abstract bool IsWarnEnabled { get; }

        public abstract bool IsErrorEnabled { get; }

        public abstract bool IsFatalEnabled { get; }

        #region ILogger Methods

        public void Trace<T>(T message)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, message, null);
            }
        }

        public void Trace(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, string.Format(format, args), null);
            } 
        }

        public void Debug<T>(T message) 
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, message, null);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, string.Format(format, args), null);
            }
        }

        public void Info<T>(T message)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, message, null);
            }
        }

        public void Info(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, string.Format(format, args), null);
            }
        }

        public void Warn<T>(T message)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, message, null);
            }
        }

        public void Warn(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, string.Format(format, args), null);
            }
        }

        public void Error<T>(T message)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, null);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, string.Format(format, args), null);
            }
        }

        public void Error<T>(T message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, exception);
            }
        }

        public void Error(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, string.Format(format, args), exception);
            }
        }

        public void Fatal<T>(T message)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, null);
            }
        }

        public void Fatal(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, string.Format(format, args), null);
            }
        }

        public void Fatal<T>(T message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, exception);
            }
        }

        public void Fatal(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, string.Format(format, args), exception);
            }
        }

        #endregion

        protected abstract void Write(LogLevel level, object message, Exception exception);
    }
}
