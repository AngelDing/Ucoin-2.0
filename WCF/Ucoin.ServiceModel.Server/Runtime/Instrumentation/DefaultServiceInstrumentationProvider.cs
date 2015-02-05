using System.Diagnostics;
using CtripSZ.ServiceModel.Core;

namespace CtripSZ.ServiceModel.Runtime
{
    public class DefaultServiceInstrumentationProvider : IServiceInstrumentationProvider
    {

        const string CATEGORY_NAME = "WCF Server";
        const string NAME = "WCF Server Performance Counter";

        private PerformanceCounter _durationCounter;
        private PerformanceCounter _avgDurationCounter;
        private PerformanceCounter _faultCounter;
        //private PerformanceCounter _averageBase;
        private PerformanceCounter _count;

       
        static DefaultServiceInstrumentationProvider()
        {
            SetupCategory();
        }

        public static void SetupCategory()
        {
            if (!PerformanceCounterCategory.Exists(CATEGORY_NAME))
            {

                var CCDC = new CounterCreationDataCollection();

                var a = new CounterCreationData();
                a.CounterType = PerformanceCounterType.RateOfCountsPerSecond64;
                a.CounterName = "Invoked Count/sec";
                CCDC.Add(a);

                var b = new CounterCreationData();
                b.CounterType = PerformanceCounterType.AverageTimer32;
                b.CounterName = "Average Duration/sec";
                //CCDC.Add(b);

                //CounterCreationData c = new CounterCreationData();
                //c.CounterType = PerformanceCounterType.AverageTimer32;
                //c.CounterName = "Message Size/sec";
                //CCDC.Add(c);

                var d = new CounterCreationData();
                d.CounterType = PerformanceCounterType.RateOfCountsPerSecond64;
                d.CounterName = "Server Fault/sec";
                CCDC.Add(d);

                var e = new CounterCreationData();
                e.CounterType = PerformanceCounterType.AverageBase;
                e.CounterName = "Invoked Count";
                //CCDC.Add(e);

                PerformanceCounterCategory.Create(CATEGORY_NAME, NAME, PerformanceCounterCategoryType.MultiInstance, CCDC);
                PerformanceCounterCategory.Create(CATEGORY_NAME + " Global", NAME, PerformanceCounterCategoryType.SingleInstance
                    , new CounterCreationDataCollection { b, e });

            }
        }

        public DefaultServiceInstrumentationProvider(string instanceName)
        {
            InitCounter(instanceName);
        }

        private void InitCounter(string instanceName)
        {
            _durationCounter = new PerformanceCounter(CATEGORY_NAME, "Invoked Count/sec", instanceName, false);

            _avgDurationCounter = new PerformanceCounter(CATEGORY_NAME + " Global", "Average Duration/sec", false);

            _faultCounter = new PerformanceCounter(CATEGORY_NAME, "Server Fault/sec", instanceName, false);

            _count = new PerformanceCounter(CATEGORY_NAME + " Global", "Invoked Count", false);
           // _MessageSizeCounter = new PerformanceCounter(CATEGORY_NAME, "Message Size/sec", instanceName, false);
        }

        public static void RemoveCategory()
        {
            if (PerformanceCounterCategory.Exists(CATEGORY_NAME))
            {
                PerformanceCounterCategory.Delete(CATEGORY_NAME);
            }
            if (PerformanceCounterCategory.Exists(CATEGORY_NAME + " Global"))
            {
                PerformanceCounterCategory.Delete(CATEGORY_NAME + " Global");
            }

        }

        public void InvokeDuration(int time)
        {
            _durationCounter.Increment();
            _count.Increment();
            _avgDurationCounter.IncrementBy(time);
           

        }

        public void Error()
        {
            _faultCounter.Increment();
        }

        public void InvokeMessageSize(int length)
        {
           // _MessageSizeCounter.IncrementBy(length);
        }


        void IInstrumentation.Setup()
        {
            SetupCategory();
        }

        void IInstrumentation.Remove()
        {
            RemoveCategory();
        }
    }
}
