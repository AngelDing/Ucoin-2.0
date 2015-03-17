using Metrics.Reporters;
using System;
using System.Collections.Generic;
using System.IO;
using Ucoin.Framework.Service;
using Ucoin.Log.Entities;
using Ucoin.Log.IServices;

namespace Ucoin.Framework.Performance
{
    public class MongoDbCSVAppender : CSVAppender
    {
        public MongoDbCSVAppender()
            : base(",")
        { }

        public override void AppendLine(DateTime timestamp, string metricType, string metricName, IEnumerable<CSVReport.Value> values)
        {
            var loggerName = string.Format("Metrics.CSV.{0}.{1}", metricType, metricName);
            var perfLog = GetLogEvent(loggerName, timestamp, metricType, metricName, values);

            var logService = ServiceLocator.GetService<ILogService>();
            logService.LogPerfInfo(perfLog);
        }

        private PerfLog GetLogEvent(string logger, DateTime timestamp, string metricType, string metricName, IEnumerable<CSVReport.Value> values)
        {
            var logEvent = GetValues(timestamp, values);
            var MetricType = CleanFileName(metricType);
            var MetricName = CleanFileName(metricName);
            var Date = timestamp.ToString("u");
            var Ticks = timestamp.Ticks.ToString("D");
            foreach (var value in values)
            {
                var k = value.Name;
                var v = value.FormattedValue;
            }
            return null;
        }

        protected virtual string CleanFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in invalid)
            {
                name = name.Replace(c, '_');
            }
            return name;
        }
    }
}
