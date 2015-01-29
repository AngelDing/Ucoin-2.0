using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Ucoin.Framework.Test
{
    public class EnumeratorCompareTest : BaseCompareTest
    {
        [Fact]
        public void compare_enumerator_complex_test()
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
                OrderId =  12,
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
            result.AreEqual.Should().BeFalse();

            var msg = result.DifferencesString;

            //var addList = result.NeedAddList;
            //addList.Count.Should().Be(1);
            //var type = typeof(HashSetClass);
            //addList.ContainsKey(type).Should().Be(true);
            //addList[type].Count.Should().Be(1);

            //var updateList = result.NeedUpdateList;
            //updateList.Count.Should().Be(1);

            //var deleteList = result.NeedDeleteList;
            //deleteList.ContainsKey(type).Should().BeTrue();
            //deleteList[type].Count.Should().Be(1);

            result.Differences.Count.Should().Be(5);
        }

        [Fact]
        public void compare_enumerator_same_test()
        {
            var obj1 = new EnumeratorWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };
            var obj2 = new EnumeratorWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };

            var set1 = new HashSetClass
            {
                Id = 1
            };
            var set2 = new HashSetClass
            {
                Id = 1
            };

            obj1.HashSetCollection.Add(set1);
            obj2.HashSetCollection.Add(set2);

            var result = CompareLogic.Compare(obj1, obj2);

            result.AreEqual.Should().BeTrue();
        }
    }
}
