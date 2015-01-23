using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace Ucoin.Framework.Test
{
    public class TimespanCompareTest : BaseCompareTest
    {
        [Fact]
        public void compare_time_span_test()
        {
            var ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            var ts2 = ts1;

            var result = CompareLogic.Compare(ts1, ts2);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void compare_time_span_negative_test()
        {
            var ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            var ts2 = DateTime.Now - DateTime.Now.AddHours(-49);
            var result = CompareLogic.Compare(ts1, ts2);
            result.AreEqual.Should().BeFalse();
        }
    }
}
