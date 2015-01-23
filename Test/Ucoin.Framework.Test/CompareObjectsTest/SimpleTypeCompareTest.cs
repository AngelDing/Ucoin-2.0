using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Ucoin.Framework.Test
{
    public class SimpleTypeCompareTest : BaseCompareTest
    {
        [Fact]
        public void compare_simple_type_with_guid_test()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = guid1;

            var result = CompareLogic.Compare(guid1, guid2);
            result.AreEqual.Should().BeTrue();

            guid2 = Guid.NewGuid();
            result = CompareLogic.Compare(guid1, guid2);
            result.AreEqual.Should().BeFalse();            
        }

        [Fact]
        public void compare_simple_type_with_string_test()
        {
            string str1 = "AA";
            string str2 = str1;

            var result = CompareLogic.Compare(str1, str2);
            result.AreEqual.Should().BeTrue();

            str2 = "BB";
            result = CompareLogic.Compare(str1, str2);
            result.AreEqual.Should().BeFalse();

            str1 = null;
            str2 = string.Empty;
            result = CompareLogic.Compare(str1, str2);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void compare_simple_type_with_decimal_test()
        {
            decimal d1 = 8;
            decimal d2 = d1;

            var result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeTrue();

            d2 = d1 + 1;
            result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeFalse();
        }

        [Fact]
        public void compare_simple_type_with_sbyte_test()
        {
            sbyte d1 = 8;
            sbyte d2 = d1;

            var result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeTrue();

            d2 = 9;
            result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeFalse();
        }

        [Fact]
        public void compare_simple_type_with_datetime_test()
        {
            DateTime d1 = DateTime.Now;
            DateTime d2 = d1;

            var result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeTrue();

            d2 = DateTime.Now.AddSeconds(1);
            result = CompareLogic.Compare(d1, d2);
            result.AreEqual.Should().BeFalse();
        }        
    }
}
