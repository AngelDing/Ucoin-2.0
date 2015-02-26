using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Xunit;
using FluentAssertions;
using Ucoin.Framework.Logging.EntLib;
using System.Collections.Specialized;
using Ucoin.Framework.Logging;

namespace Ucoin.Logging.Test.EntLib
{
    public class EntLibLoggerAdapterTests
    {
        [Fact]
        public void logging_entlib_init_defaults_test()
        {
            var adapter = new EntLibLoggerAdapter();
            adapter.ExceptionFormat.Should().Be(LoggerSettings.DEFAULTEXCEPTIONFORMAT);
            adapter.DefaultPriority.Should().Be(LoggerSettings.DEFAULTPRIORITY);

            adapter = new EntLibLoggerAdapter(null);
            adapter.ExceptionFormat.Should().Be(LoggerSettings.DEFAULTEXCEPTIONFORMAT);
            adapter.DefaultPriority.Should().Be(LoggerSettings.DEFAULTPRIORITY);
        }

        [Fact]
        public void logging_entlib_init_with_properties_test()
        {
            var props = new NameValueCollection();
            props["exceptionFormat"] = "$(exception.message)";
            props["priority"] = "4";
            var a = new EntLibLoggerAdapter(props);
            a.ExceptionFormat.Should().Be("$(exception.message)");
            a.DefaultPriority.Should().Be(4);
        }

        [Fact]
        public void logging_entlib_caches_loggers_test()
        {
            var severityFilter = new SeverityFilter(null, TraceEventType.Critical | TraceEventType.Error);
            var a = CreateTestEntLibLoggerAdapter(severityFilter);

            var log = a.GetLogger(this.GetType());
            a.GetLogger(this.GetType()).Should().Be(log);
        }

        [Fact]
        public void logging_entlib_log_message_test()
        {
            var severityFilter = new SeverityFilter(null, TraceEventType.Critical | TraceEventType.Error);
            var a = CreateTestEntLibLoggerAdapter(severityFilter);
            var ex = new Exception("errormessage");

            var log = a.GetLogger(this.GetType());

            // not logged due to severity filter 
            a.LastLogEntry = null;
            log.Trace("Message1", ex);
            a.LastLogEntry.Should().BeNull();

            // logged, passes severity filter
            a.LastLogEntry = null;
            log.Error("Message2", ex);
            a.LastLogEntry.Severity.Should().Be(TraceEventType.Error);
            a.LastLogEntry.Message.Should().Be("Message2");
            a.LastLogEntry.Priority.Should().Be(a.DefaultPriority);
            a.LastLogEntry.Categories.Count.Should().Be(1);
            a.LastLogEntry.CategoriesStrings[0].Should().Be(this.GetType().FullName);
            var exceptString = "Exception[ message = errormessage, source = , targetsite = , stacktrace =  ]";
            a.LastLogEntry.ErrorMessages.Trim().Should().Be(exceptString);
        }

        #region TestEntLibLoggerAdapter

        private static TestEntLibLoggerAdapter CreateTestEntLibLoggerAdapter(ILogFilter filter)
        {
            LogWriter logWriter = new LogWriter(
                new ILogFilter[] { filter }
                , new LogSource[] { new LogSource("logSource") }
                , new LogSource("defaultLogSource")
                , new LogSource("notProcessedLogSource")
                , new LogSource("errorsLogSource")
                , "DefaultCategory"
                , true
                , true
                );

            return new TestEntLibLoggerAdapter(logWriter);
        }

        private class TestEntLibLoggerAdapter : EntLibLoggerAdapter
        {
            public LogEntry LastLogEntry;

            public TestEntLibLoggerAdapter(LogWriter logWriter)
                : base(logWriter)
            {
            }

            protected override ILogger CreateLogger(string name, LogWriter logWriter)
            {
                return new TestEntLibLogger(this, name, logWriter);
            }

            private class TestEntLibLogger : EntLibLogger
            {
                private readonly TestEntLibLoggerAdapter owner;

                public TestEntLibLogger(TestEntLibLoggerAdapter owner, string category, LogWriter logWriter)
                    : base(category, logWriter)
                {
                    this.owner = owner;
                }

                protected override void WriteLog(LogEntry log)
                {
                    owner.LastLogEntry = log;
                    base.WriteLog(log);
                }
            }
        }

        #endregion
    }
}
