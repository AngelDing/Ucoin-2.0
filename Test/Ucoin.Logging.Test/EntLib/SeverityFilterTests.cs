using System;
using System.Diagnostics;
using Ucoin.Framework.Logging.EntLib;
using Xunit;
using FluentAssertions;
using System.Collections.Specialized;

namespace Ucoin.Logging.Test.EntLib
{
    public class SeverityFilterTests
    {
        [Fact]
        public void logging_entlib_default_settings_test()
        {
            var sf = new SeverityFilter(null);
            sf.Name.Should().Be("Severity Filter");
            sf.SeverityMask.Should().Be(Int32.MaxValue);
        }

        [Fact]
        public void logging_entlib_set_properties_test()
        {
            var sf = new SeverityFilter("name", 4);
            sf.Name.Should().Be("name");
            sf.SeverityMask.Should().Be(4);

            var props = new NameValueCollection();
            props["Name"] = "name";
            props["SeverityMask"] = "4";
            sf = new SeverityFilter(props);
            sf.Name.Should().Be("name");
            sf.SeverityMask.Should().Be(4);
        }

        [Fact]
        public void logging_entlib_filter_by_mask_test()
        {
            var sf = new SeverityFilter("name", 6);
            sf.ShouldLog((TraceEventType)0).Should().BeTrue();
            sf.ShouldLog((TraceEventType)1).Should().BeFalse();
            sf.ShouldLog((TraceEventType)2).Should().BeTrue();
            sf.ShouldLog((TraceEventType)4).Should().BeTrue();
            sf.ShouldLog((TraceEventType)7).Should().BeFalse();
            sf.ShouldLog((TraceEventType)255).Should().BeFalse();
        }
    }
}
