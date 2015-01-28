using System.Collections.Generic;
using Xunit;

namespace Ucoin.Framework.ObjectMapper.Test
{
    public static class Helper
    {
        public static void AreSequentialEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null)
            {
                Assert.Null(second);
                return;
            }
            if (second == null)
            {
                Assert.Null(first);
                return;
            }
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext())
            {
                Assert.True(secondEnumerator.MoveNext());
                Assert.Equal(firstEnumerator.Current, secondEnumerator.Current);
            }
            Assert.False(secondEnumerator.MoveNext());
        }
    }
}
