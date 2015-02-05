using System;
using CtripSZ.ServiceModel.Core;
using Microsoft.Practices.Unity;
using System.ServiceModel;
using Common.Logging;

namespace CtripSZ.ServiceModel.Runtime
{
    static class Instrumentation
    {

        static readonly Lazy<IServiceInstrumentationProvider> _serviceInstrumentationProvider =
            new Lazy<IServiceInstrumentationProvider>(LoadServiceInstrumentation);

        static IServiceInstrumentationProvider LoadServiceInstrumentation()
        {
            var instance = AppDomain.CurrentDomain.FriendlyName;
            if (Container.Current.IsRegistered<IServiceInstrumentationProvider>())
                return Container.Current.Resolve<IServiceInstrumentationProvider>(
                    new ParameterOverride("instanceName", instance));
            return null;
        }

        static readonly Lazy<ILog> _lazyLogger = new Lazy<ILog>(GetLogger);


        static ILog GetLogger()
        {
            const string loggerName = "performance";
            return LogManager.GetLogger(loggerName);
        }

        private static void WriteLog(int time)
        {
            if (_lazyLogger.Value == null || minValue == 0 || time < minValue)
                return;
            var name = (OperationContext.Current.IncomingMessageHeaders).Action;

            if (time > minValue * 5)
                _lazyLogger.Value.Error(string.Format("Performance\t{0} duration:{1}ms", name, time));
            else
                _lazyLogger.Value.Warn(string.Format("Performance\t{0} duration:{1}ms", name, time));
        }

        static readonly int minValue = 10;

        private static IServiceInstrumentationProvider ServiceInstrumentation
        {
            get
            {
                return _serviceInstrumentationProvider.Value;
            }
        }

        public static void InvokeDuration(int time)
        {
            WriteLog(time);

            if (ServiceInstrumentation != null)
                ServiceInstrumentation.InvokeDuration(time);
        }

        public static void Error()
        {
            if (ServiceInstrumentation != null)
                ServiceInstrumentation.Error();
        }

        public static void InvokeMessageSize(int length)
        {
            if (ServiceInstrumentation != null)
                ServiceInstrumentation.InvokeMessageSize(length);
        }

    }
}
