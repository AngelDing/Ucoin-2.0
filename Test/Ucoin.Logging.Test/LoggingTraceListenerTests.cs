using Xunit;
using FluentAssertions;
using Ucoin.Framework.Logging;
using Ucoin.Framework.Logging.Simple;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Ucoin.Logging.Test
{
    public class LoggingTraceListenerTests
    {
        public LoggingTraceListenerTests()
        {
            LogManager.Reset();
        }

        [Fact]
        public void logging_trace_listener_test()
        {
            var adapter = new CapturingLoggerAdapter();
            LogManager.Adapter = adapter;

            var listener = new LoggingTraceListener();
            listener.DefaultTraceEventType = (TraceEventType)0xFFFF;

            AssertExpectedLogLevel(listener, TraceEventType.Start, LogLevel.Trace);
            AssertExpectedLogLevel(listener, TraceEventType.Stop, LogLevel.Trace);
            AssertExpectedLogLevel(listener, TraceEventType.Suspend, LogLevel.Trace);
            AssertExpectedLogLevel(listener, TraceEventType.Resume, LogLevel.Trace);
            AssertExpectedLogLevel(listener, TraceEventType.Transfer, LogLevel.Trace);
            AssertExpectedLogLevel(listener, TraceEventType.Verbose, LogLevel.Debug);
            AssertExpectedLogLevel(listener, TraceEventType.Information, LogLevel.Info);
            AssertExpectedLogLevel(listener, TraceEventType.Warning, LogLevel.Warn);
            AssertExpectedLogLevel(listener, TraceEventType.Error, LogLevel.Error);
            AssertExpectedLogLevel(listener, TraceEventType.Critical, LogLevel.Fatal);

            adapter.Clear();
            listener.DefaultTraceEventType = TraceEventType.Warning;
            listener.Write("some message", "some category");
            var logName = adapter.LastEvent.Source.ArgumentEntity.LogName;

            logName.Should().Be(string.Format("{0}.{1}", listener.Name, "some category"));
            adapter.LastEvent.Level.Should().Be(LogLevel.Warn);
            adapter.LastEvent.RenderedMessage.Should().Be("some message");
            adapter.LastEvent.Exception.Should().BeNull();
        }

        [Fact]
        public void logging_trace_listener_null_category_test()
        {
            var adapter = new CapturingLoggerAdapter();
            LogManager.Adapter = adapter;
            var listener = new LoggingTraceListener();

            listener.DefaultTraceEventType = TraceEventType.Warning;
            listener.Write("some message", null);

            var logName = adapter.LastEvent.Source.ArgumentEntity.LogName;
            var exceptName =string.Format("{0}.{1}", listener.Name, ""); 
            logName.Should().Be(exceptName);
            adapter.LastEvent.Level.Should().Be(LogLevel.Warn);
            adapter.LastEvent.RenderedMessage.Should().Be("some message");
            adapter.LastEvent.Exception.Should().BeNull();
        }        

        [Fact]
        public void logging_not_log_below_filter_level_test()
        {
            var adapter = new CapturingLoggerAdapter();
            LogManager.Adapter = adapter;

            var listener = new LoggingTraceListener();
            listener.Filter = new EventTypeFilter(SourceLevels.Warning);
            adapter.Clear();
            listener.TraceEvent(null, "sourceName", TraceEventType.Information, -1, "format {0}", "Information");
            adapter.LastEvent.Should().BeNull();

            AssertExpectedLogLevel(listener, TraceEventType.Warning, LogLevel.Warn);
            AssertExpectedLogLevel(listener, TraceEventType.Error, LogLevel.Error);
        }

        [Fact]
        public void logging_trace_listener_default_settings_test()
        {
            var listener = new LoggingTraceListener();

            AssertDefaultSettings(listener);
        }

        [Fact]
        public void logging_trace_listener_processes_properties_test()
        {
            var props = new NameValueCollection();
            props["Name"] = "TestName";
            props["DefaultTraceEventType"] = TraceEventType.Information.ToString().ToLower();
            props["LoggerNameFormat"] = "{0}-{1}";
            var listener = new LoggingTraceListener(props);

            listener.Name.Should().Be("TestName");
            listener.DefaultTraceEventType.Should().Be(TraceEventType.Information);
            listener.LoggerNameFormat.Should().Be("{0}-{1}");
        }

        [Fact]
        public void logging_trace_listener_processes_initialize_data_test()
        {
            // null results in default settings
            var listener = new LoggingTraceListener((string)null);
            AssertDefaultSettings(listener);

            // string.Empty results in default settings
            listener = new LoggingTraceListener(string.Empty);
            AssertDefaultSettings(listener);

            // values are trimmed and case-insensitive, empty values ignored
            listener = new LoggingTraceListener("; DefaultTraceeventtype   =warninG; loggernameFORMAT= {listenerName}-{sourceName}\t; Name =  TestName\t; ");
            listener.Name.Should().Be("TestName");
            listener.DefaultTraceEventType.Should().Be(TraceEventType.Warning);
            listener.LoggerNameFormat.Should().Be("{listenerName}-{sourceName}");
        }

        private void AssertExpectedLogLevel(LoggingTraceListener listener, TraceEventType eType, LogLevel level)
        {
            var adapter = (CapturingLoggerAdapter)LogManager.Adapter;
            adapter.Clear();
            listener.TraceEvent(null, "sourceName " + eType, eType, -1, "format {0}", eType);
            var logName = adapter.LastEvent.Source.ArgumentEntity.LogName;
            var exceptName = string.Format("{0}.{1}", listener.Name, "sourceName " + eType);
            logName.Should().Be(exceptName);
            adapter.LastEvent.Level.Should().Be(level);
            adapter.LastEvent.RenderedMessage.Should().Be("format " + eType);
            adapter.LastEvent.Exception.Should().BeNull();
        }

        private void AssertDefaultSettings(LoggingTraceListener listener)
        {
            listener.Name.Should().Be("Diagnostics");
            listener.DefaultTraceEventType.Should().Be(TraceEventType.Verbose);
            listener.LoggerNameFormat.Should().Be("{listenerName}.{sourceName}");
        }
    }
}
