using System.Collections.Generic;

namespace Ucoin.Log.Entities
{
    public class SysPerfLog : BasePerfLog
    {
        public string MetricName { get; set; }

        public string MetricType { get; set; }

        public string AppCodeType { get; set; }

        public IMetricValue MetricValue { get; set; }
    }
}
