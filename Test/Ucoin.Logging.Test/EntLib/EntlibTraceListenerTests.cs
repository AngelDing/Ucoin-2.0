using Xunit;
using Ucoin.Logging.Test;
using Ucoin.Framework.Logging;
using EntLitLogging = Microsoft.Practices.EnterpriseLibrary.Logging;
using Ucoin.Framework.Logging.EntLib;
using FluentAssertions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.Linq;

namespace Ucoin.Logging.Test.EntLib
{
    public class CommonLoggingEntlibTraceListenerTests
    {
        [Fact]
        public void logging_entlib_trace_listener_tests()
        {
            var adapter = new CapturingLoggerAdapter();
            LogManager.Adapter = adapter;

            // force entlib logging init
            EntLitLogging.Logger.SetLogWriter(new LogWriterFactory().Create());
            EntLitLogging.Logger.Write("init");

            // ensure external configuration didn't change
            LogEntry logEntry = new LogEntry();
            logEntry.Categories.Add("mycategory");
            logEntry.Message = "testmessage";

            // Change to EL 6 (no longer uses Unity) so need to get listener via LogWriter
            var allEventsSource = EntLitLogging.Logger.Writer.GetMatchingTraceSources(logEntry).First(source => "All Events".Equals(source.Name));
            var listener = (CustomTraceListener)allEventsSource.Listeners.First(l => l is CustomTraceListener);
            listener.Name.StartsWith("Test Capturing Listener").Should().BeTrue();

            var formatter = listener.Formatter;
            string s = formatter.Format(logEntry);
            s.Should().Be("Category: mycategory, Message: testmessage");
            
            // note that output depends on the formatter configured for the entlib listener!
            EntLitLogging.Logger.Write("message1");
            adapter.LastEvent.RenderedMessage.Should().Be("Category: General, Message: message1");
            adapter.LastEvent.Level.Should().Be(LogLevel.Info);

            EntLitLogging.Logger.Write("message2", "custom category", -1, -1, TraceEventType.Warning);
            adapter.LastEvent.RenderedMessage.Should().Be("Category: custom category, Message: message2");
            adapter.LastEvent.Source.ArgumentEntity.LogName.Should().Be("Test Capturing Listener/All Events");
            adapter.LastEvent.Level.Should().Be(LogLevel.Warn);
        }
    }
}