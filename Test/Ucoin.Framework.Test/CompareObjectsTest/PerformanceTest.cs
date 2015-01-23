using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Ucoin.Framework.Test
{
    public class PerformanceTest : BaseCompareTest
    {
        [Fact]
        public void compare_performance_test()
        {
            int max = 10000;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < max; i++)
            {
                var obj1 = new EnumeratorWrapper
                {
                    StatusId = 1,
                    Name = "Paul",
                    CreateDate = DateTime.Now
                };
                var obj2 = new EnumeratorWrapper
                {
                    StatusId = 1,
                    Name = "Paull",
                    Amount = 100
                };

                var set1 = new HashSetClass
                {
                    Id = 1,
                    Name = "aa",
                    OrderId = 12,
                    HashSetWrapper = obj1
                };
                var set2 = new HashSetClass
                {
                    Id = 1,
                    Name = "bb",
                    HashSetWrapper = obj1
                };
                var set3 = new HashSetClass
                {
                    Id = 2,
                    Name = "aa",
                    OrderId = 12
                };
                var set4 = new HashSetClass
                {
                    Id = 3,
                    Name = "bb"
                };

                obj1.HashSetCollection.Add(set1);
                obj1.HashSetCollection.Add(set3);
                obj2.HashSetCollection.Add(set2);
                obj2.HashSetCollection.Add(set4);

                var result = CompareLogic.Compare(obj1, obj2);
            }

            watch.Stop();
            watch.ElapsedMilliseconds.Should().BeLessThan(2 * 1000);
        }
    }
}
