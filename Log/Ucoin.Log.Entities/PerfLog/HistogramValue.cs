
namespace Ucoin.Log.Entities
{
    public class HistogramValue : IMetricValue
    {
        public double Last { get; set; }

        public string LastUserValue { get; set; }

        public double Min { get; set; }

        public string MinUserValue { get; set; }

        public double Max { get; set; }

        public string MaxUserValue { get; set; }

        public double Mean { get; set; }

        public double StdDev { get; set; }

        public double Median { get; set; }

        public double Percent75 { get; set; }

        public double Percent95 { get; set; }

        public double Percent98 { get; set; }

        public double Percent99 { get; set; }

        public double Percent999 { get; set; }

        public int SampleSize { get; set; }
    }
}
