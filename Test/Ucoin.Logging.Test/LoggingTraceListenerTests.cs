using Xunit;
using FluentAssertions;
using Ucoin.Framework.Logging;
using Ucoin.Framework.Logging.Simple;
using System.Diagnostics;

namespace Ucoin.Logging.Test
{
    public class LoggingTraceListenerTests
    {
        public LoggingTraceListenerTests()
        {
            LogManager.Reset();
        }

        [Fact]
        public void LogsUsingCommonLogging()
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
        public void AcceptsNullCategory()
        {
            var adapter = new CapturingLoggerAdapter();
            LogManager.Adapter = adapter;
            var listener = new LoggingTraceListener();

            listener.DefaultTraceEventType = TraceEventType.Warning;
            listener.Write("some message", null);
            Assert.AreEqual(string.Format("{0}.{1}", listener.Name, ""), adapter.LastEvent.Source.Name);
            Assert.AreEqual(LogLevel.Warn, adapter.LastEvent.Level);
            Assert.AreEqual("some message", adapter.LastEvent.RenderedMessage);
            Assert.AreEqual(null, adapter.LastEvent.Exception);
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

        //[Test]
        //public void DoesNotLogBelowFilterLevel()
        //{
        //    CapturingLoggerFactoryAdapter factoryAdapter = new CapturingLoggerFactoryAdapter();
        //    LogManager.Adapter = factoryAdapter;

        //    CommonLoggingTraceListener l = new CommonLoggingTraceListener();
        //    l.Filter = new EventTypeFilter(SourceLevels.Warning);
        //    factoryAdapter.ClearLastEvent();
        //    l.TraceEvent(null, "sourceName", TraceEventType.Information, -1, "format {0}", "Information");
        //    Assert.AreEqual(null, factoryAdapter.LastEvent);

        //    AssertExpectedLogLevel(l, TraceEventType.Warning, LogLevel.Warn);
        //    AssertExpectedLogLevel(l, TraceEventType.Error, LogLevel.Error);
        //}

        //[Test]
        //public void DefaultSettings()
        //{
        //    CommonLoggingTraceListener l = new CommonLoggingTraceListener();

        //    AssertDefaultSettings(l);
        //}

        //[Test]
        //public void ProcessesProperties()
        //{
        //    CommonLoggingTraceListener l;

        //    NameValueCollection props = new NameValueCollection();
        //    props["Name"] = "TestName";
        //    props["DefaultTraceEventType"] = TraceEventType.Information.ToString().ToLower();
        //    props["LoggerNameFormat"] = "{0}-{1}";
        //    l = new CommonLoggingTraceListener(props);

        //    Assert.AreEqual("TestName", l.Name);
        //    Assert.AreEqual(TraceEventType.Information, l.DefaultTraceEventType);
        //    Assert.AreEqual("{0}-{1}", l.LoggerNameFormat);
        //}

        //[Test]
        //public void ProcessesInitializeData()
        //{
        //    CommonLoggingTraceListener l;

        //    // null results in default settings
        //    l = new CommonLoggingTraceListener((string)null);
        //    AssertDefaultSettings(l);

        //    // string.Empty results in default settings
        //    l = new CommonLoggingTraceListener(string.Empty);
        //    AssertDefaultSettings(l);

        //    // values are trimmed and case-insensitive, empty values ignored
        //    l = new CommonLoggingTraceListener("; DefaultTraceeventtype   =warninG; loggernameFORMAT= {listenerName}-{sourceName}\t; Name =  TestName\t; ");
        //    Assert.AreEqual("TestName", l.Name);
        //    Assert.AreEqual(TraceEventType.Warning, l.DefaultTraceEventType);
        //    Assert.AreEqual("{listenerName}-{sourceName}", l.LoggerNameFormat);
        //}

        //private void AssertDefaultSettings(CommonLoggingTraceListener l)
        //{
        //    Assert.AreEqual("Diagnostics", l.Name);
        //    Assert.AreEqual(TraceEventType.Verbose, l.DefaultTraceEventType);
        //    Assert.AreEqual("{listenerName}.{sourceName}", l.LoggerNameFormat);
        //}
    }
}
