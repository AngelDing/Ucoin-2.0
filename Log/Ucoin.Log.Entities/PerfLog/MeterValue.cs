using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucoin.Log.Entities
{
    public class MeterValue : IMetricValue
    {
        public long Count { get; set; }

        public double MeanRate { get; set; }

        public double OneMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public double FifteenMinuteRate { get; set; }
    }
}
