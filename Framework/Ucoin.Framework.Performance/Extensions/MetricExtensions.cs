using Metrics.Reporters;
using Metrics.Reports;
using System;

namespace Ucoin.Framework.Performance
{
    public static class MetricExtensions
    {
        public static MetricsReports WithMongoDbReporter(this MetricsReports reports, TimeSpan interval)
        {
            reports.WithReport(new CSVReport(new MongoDbCSVAppender()), interval);
            return reports;
        }
    }
}
