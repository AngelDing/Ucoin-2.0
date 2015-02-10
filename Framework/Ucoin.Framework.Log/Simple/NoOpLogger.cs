using System;

namespace Ucoin.Framework.Logging.Simple
{
    public sealed class NoOpLogger : ILogger
    {
        public void Trace<T>(T message)
        {
        }

        public void Trace(string format, params object[] args)
        {
        }

        public void Debug<T>(T message)
        {
        }

        public void Debug(string format, params object[] args)
        {
        }

        public void Info<T>(T message)
        {
        }

        public void Info(string format, params object[] args)
        {
        }

        public void Warn<T>(T message)
        {
        }

        public void Warn(string format, params object[] args)
        {
        }

        public void Error<T>(T message)
        {
        }

        public void Error(string format, params object[] args)
        {
        }

        public void Error<T>(T message, Exception exception)
        {
        }

        public void Error(string format, Exception exception, params object[] args)
        {
        }

        public void Fatal<T>(T message)
        {
        }

        public void Fatal(string format, params object[] args)
        {
        }

        public void Fatal<T>(T message, Exception exception)
        {
        }

        public void Fatal(string format, Exception exception, params object[] args)
        {
        }
    }
}
