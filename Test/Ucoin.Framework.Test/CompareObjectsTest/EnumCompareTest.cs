using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace Ucoin.Framework.Test
{
    public class EnumCompareTest : BaseCompareTest
    {
        [Fact]
        public void compare_enum_different_test()
        {
            Officer officer1 = new Officer();
            officer1.Name = "Greg";
            officer1.Type = Deck.Engineering;

            Officer officer2 = new Officer();
            officer2.Name = "John";
            officer2.Type = Deck.AstroPhysics;

            var result = CompareLogic.Compare(officer1, officer2);

            result.Differences.Count.Should().Be(2);
            result.DifferencesString.Contains("Deck").Should().BeTrue();
        }

        [Fact]
        public void compare_enum_same_test()
        {
            Officer officer1 = new Officer();
            officer1.Name = "Greg";
            officer1.Type = Deck.Engineering;

            Officer officer2 = new Officer();
            officer2.Name = "Greg";
            officer2.Type = Deck.Engineering;

            var result = CompareLogic.Compare(officer1, officer2);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void compare_enum_nullable_same_test()
        {
            Officer officer1 = new Officer();
            officer1.Name = "Greg";
            officer1.Type2 = null;

            Officer officer2 = new Officer();
            officer2.Name = "Greg";
            officer2.Type2 = null;

            var result = CompareLogic.Compare(officer1, officer2);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void TestNullableEnumerationNegative()
        {
            Officer officer1 = new Officer();
            officer1.Name = "Greg";
            officer1.Type2 = null;

            Officer officer2 = new Officer();
            officer2.Name = "Greg";
            officer2.Type2 = Deck.AstroPhysics;

            var result = CompareLogic.Compare(officer1, officer2);
            result.AreEqual.Should().BeFalse();
        }
    }
}
