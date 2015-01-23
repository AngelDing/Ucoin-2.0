using System;
using Xunit;
using FluentAssertions;

namespace Ucoin.Framework.Test
{
    public class ClassCompareTest : BaseCompareTest
    {
        #region Null Tests
        [Fact]
        public void compare_class_null_objects_test()
        {
            Person p1 = null;
            Person p2 = null;
            var result = CompareLogic.Compare(p1, p2);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void compare_class_one_object_null_test()
        {
            Person p1 = null;
            Person p2 = new Person();
            CompareLogic.Compare(p1, p2).AreEqual.Should().BeFalse();
            CompareLogic.Compare(p2, p1).AreEqual.Should().BeFalse();
        }

        [Fact]
        public void compare_class_second_object_null_test()
        {
            Person p1 = new Person();
            Person p2 = null;
            CompareLogic.Compare(p1, p2).AreEqual.Should().BeFalse();
            CompareLogic.Compare(p2, p1).AreEqual.Should().BeFalse();
        }

        #endregion

        [Fact]
        public void compare_class_common_test()
        {
            var p1 = new Person
            {
                Name = "jacky",
                Phone = "123"
            };
            var p2 = new Person
            {
                Name = "jacky",
                Phone = "123"
            };
            var result = CompareLogic.Compare(p1, p2);
            result.AreEqual.Should().BeTrue();
            p2.Name = "jacky1";
            result = CompareLogic.Compare(p1, p2);
            result.AreEqual.Should().BeFalse();
        }
    }
}
