using Xunit;
using FluentAssertions;
using Ucoin.Framework.Logging.Simple;
using Ucoin.Framework.Logging;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Ucoin.Logging.Test
{
    public class TraceLoggerTests
    {
        public TraceLoggerTests()
        {
            LogManager.Reset();
        }

        [Fact]
        public void logging_uses_trace_source_by_direct_test()
        {
            // just ensure, that <system.diagnostics> is configured for our test
            Trace.Refresh();
            var ts = new TraceSource("TraceLoggerTests", SourceLevels.All);
            ts.Listeners.Count.Should().Be(1);
            ts.Listeners[0].GetType().Should().Be(typeof(CapturingTraceListener));

            CapturingTraceListener.Events.Clear();
            ts.TraceEvent(TraceEventType.Information, 0, "message");
            CapturingTraceListener.Events[0].EventType.Should().Be(TraceEventType.Information);
            CapturingTraceListener.Events[0].FormattedMessage.Should().Be("message");
            Trace.Refresh();
        }

        [Fact]
        public void logging_uses_trace_source_by_adapter_test()
        {
            Trace.Refresh();
            var props = new NameValueCollection();
            props["USETRACESOURCE"] = "TRUE";
            var adapter = new TraceLoggerAdapter(props);
            adapter.ArgumentEntity.ShowDateTime = false;
            LogManager.Adapter = adapter;

            var log = LogManager.GetLogger("TraceLoggerTests");
            log.Warn("info {0}", "arg");

            var capEvent = CapturingTraceListener.Events[0];
            capEvent.EventType.Should().Be(TraceEventType.Warning);
            capEvent.FormattedMessage.Should().Be("[WARN]  TraceLoggerTests - info arg");
            Trace.Refresh();
        }
    }
}
