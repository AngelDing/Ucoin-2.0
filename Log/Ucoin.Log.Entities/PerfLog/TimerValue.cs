
namespace Ucoin.Log.Entities
{
    public class TimerValue : IMetricValue
    {
        public MeterValue Rate { get; set; }

        public HistogramValue Histogram { get; set; }

        public long ActiveSessions { get; set; }     
    }
}
