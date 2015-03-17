using Metrics;
using System;
using System.Web;
using System.Web.Mvc;
using Ucoin.Framework.Performance;

namespace Ucoin.Authority.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (ConfigInfo.Value.PerformanceEnabled == true)
            {
                Metric.Config
                    .WithHttpEndpoint("http://localhost:1234/")
                    .WithAllCounters()
                    //.WithReporting(report => report.WithCSVReports(@"E:\CtripSZ\Asp.Net\GroupTour_WebApi_Demo\MetricsCSV\", TimeSpan.FromSeconds(10)));
                    .WithReporting(report => report.WithMongoDbReporter(TimeSpan.FromSeconds(10)));
            }

            filters.Add(new HandleErrorAttribute());
            //添加性能標記
            filters.Add(new MvcPerformanceAttribute());
        }
    }
}
