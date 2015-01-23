using Xunit;
using FluentAssertions;

namespace Ucoin.Framework.Test
{
    public class StringCompareTest : BaseCompareTest
    {
        [Fact]
        public void compare_string_empty_and_null_different()
        {
            CompareLogic.Config.TreatStringEmptyAndNullTheSame = false;
            var result = CompareLogic.Compare(string.Empty, null);
            result.AreEqual.Should().BeFalse();
        }

        [Fact]
        public void compare_string_empty_and_null_equal()
        {
            CompareLogic.Config.TreatStringEmptyAndNullTheSame = true;
            var result = CompareLogic.Compare(string.Empty, null);
            result.AreEqual.Should().BeTrue();
        }

        [Fact]
        public void compare_string_common_test()
        {
            var result = CompareLogic.Compare("AA", "BB");
            result.AreEqual.Should().BeFalse();
            result = CompareLogic.Compare("AA", "AA");
            result.AreEqual.Should().BeTrue();
        }
    }
}
