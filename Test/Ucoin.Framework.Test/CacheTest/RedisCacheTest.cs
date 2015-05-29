﻿using Xunit;
using System;
using System.Linq;
using StackExchange.Redis;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.Cache;
using FluentAssertions;

namespace Ucoin.Framework.Test.Caching
{
	public class RedisCacheTest : IDisposable
	{
        private readonly ICacheManager cacheManager;

        public RedisCacheTest()
		{
            cacheManager = CacheHelper.RedisCache;
		}

        [Fact]
        public void cache_add_item_to_redis()
        {
            cacheManager.Set("my Key", "my value");

            cacheManager.Get<string>("my Key").Should().NotBeEmpty();
        }

        //[Fact]
        //public void Add_Complex_Item_To_Redis_Database()
        //{
        //    TestClass<DateTime> testobject = new TestClass<DateTime>();

        //    var added = Sut.Add("my Key", testobject);

        //    var result = Db.StringGet("my Key");

        //    Assert.True(added);
        //    Assert.NotNull(result);

        //    var obj = Serializer.Deserialize<TestClass<DateTime>>(result);

        //    Assert.True(Db.KeyExists("my Key"));
        //    Assert.NotNull(obj);
        //    Assert.Equal(testobject.Key, obj.Key);
        //    Assert.Equal(testobject.Value, obj.Value);
        //}

        //[Fact]
        //public void Add_Multiple_Object_With_A_Single_Roundtrip_To_Redis_Must_Store_Data_Correctly_Into_Database()
        //{
        //    IList<Tuple<string, string>> values = new List<Tuple<string, string>>();
        //    values.Add(new Tuple<string, string>("key1", "value1"));
        //    values.Add(new Tuple<string, string>("key2", "value2"));
        //    values.Add(new Tuple<string, string>("key3", "value3"));

        //    bool added = Sut.AddAll(values);

        //    Assert.True(added);

        //    Assert.True(Db.KeyExists("key1"));
        //    Assert.True(Db.KeyExists("key2"));
        //    Assert.True(Db.KeyExists("key3"));

        //    Assert.Equal(Serializer.Deserialize<string>(Db.StringGet("key1")), "value1");
        //    Assert.Equal(Serializer.Deserialize<string>(Db.StringGet("key2")), "value2");
        //    Assert.Equal(Serializer.Deserialize<string>(Db.StringGet("key3")), "value3");
        //}

        //[Fact]
        //public void Get_All_Should_Return_All_Database_Keys()
        //{
        //    var values = Builder<TestClass<string>>
        //                .CreateListOfSize(5)
        //                .All()
        //                .Build();
        //    values.ForEach(x => Db.StringSet(x.Key, Serializer.Serialize(x.Value)));

        //    IDictionary<string, string> result = Sut.GetAll<string>(new[] { values[0].Key, values[1].Key, values[2].Key, "notexistingkey" });

        //    Assert.True(result.Count() == 4);
        //    Assert.Equal(result[values[0].Key], values[0].Value);
        //    Assert.Equal(result[values[1].Key], values[1].Value);
        //    Assert.Equal(result[values[2].Key], values[2].Value);
        //    Assert.Null(result["notexistingkey"]);
        //}

        //[Fact]
        //public void Get_With_Complex_Item_Should_Return_Correct_Value()
        //{
        //    var value = Builder<ComplexClassForTest<string, string>>
        //            .CreateListOfSize(1)
        //            .All()
        //            .Build().First();

        //    Db.StringSet(value.Item1, Serializer.Serialize(value));

        //    var cachedObject = Sut.Get<ComplexClassForTest<string, string>>(value.Item1);

        //    Assert.NotNull(cachedObject);
        //    Assert.Equal(value.Item1, cachedObject.Item1);
        //    Assert.Equal(value.Item2, cachedObject.Item2);
        //}

        //[Fact]
        //public void Remove_All_Should_Remove_All_Specified_Keys()
        //{
        //    var values = Builder<TestClass<string>>
        //                .CreateListOfSize(5)
        //                .All()
        //                .Build();
        //    values.ForEach(x => Db.StringSet(x.Key, x.Value));

        //    Sut.RemoveAll(values.Select(x => x.Key));

        //    foreach (var value in values)
        //    {
        //        Assert.False(Db.KeyExists(value.Key));
        //    }
        //}

        //[Fact]
        //public void Search_With_Valid_Start_With_Pattern_Should_Return_Correct_Keys()
        //{
        //    var values = Builder<TestClass<string>>
        //                .CreateListOfSize(20)
        //                .Build();
        //    values.ForEach(x => Db.StringSet(x.Key, x.Value));

        //    var key = Sut.SearchKeys("Key1*").ToList();

        //    Assert.True(key.Count == 11);
        //}

        //[Fact]
        //public void Exist_With_Valid_Object_Should_Return_The_Correct_Instance()
        //{
        //    var values = Builder<TestClass<string>>
        //            .CreateListOfSize(2)
        //            .Build();
        //    values.ForEach(x => Db.StringSet(x.Key, x.Value));

        //    Assert.True(Sut.Exists(values[0].Key));
        //}

        //[Fact]
        //public void Exist_With_Not_Valid_Object_Should_Return_The_Correct_Instance()
        //{
        //    var values = Builder<TestClass<string>>
        //            .CreateListOfSize(2)
        //            .Build();
        //    values.ForEach(x => Db.StringSet(x.Key, x.Value));

        //    Assert.False(Sut.Exists("this key doesn not exist into redi"));
        //}

        //[Fact]
        //public void SetAdd_With_An_Existing_Key_Should_Return_Valid_Data()
        //{
        //    var values = Builder<TestClass<string>>
        //                .CreateListOfSize(5)
        //                .All()
        //                .Build();

        //    values.ForEach(x =>
        //    {
        //        Db.StringSet(x.Key, Serializer.Serialize(x.Value));
        //        Sut.SetAdd("MySet", x.Key);
        //    });

        //    var keys = Db.SetMembers("MySet");

        //    Assert.Equal(keys.Length, values.Count);
        //}

        //[Fact]
        //public void SetMember_With_Valid_Data_Should_Return_Correct_Keys()
        //{
        //    var values = Builder<TestClass<string>>
        //                .CreateListOfSize(5)
        //                .All()
        //                .Build();

        //    values.ForEach(x =>
        //    {
        //        Db.StringSet(x.Key, Serializer.Serialize(x.Value));
        //        Db.SetAdd("MySet", x.Key);
        //    });

        //    var keys = Sut.SetMember("MySet");

        //    Assert.Equal(keys.Length, values.Count);
        //}

		public void Dispose()
		{
            cacheManager.ClearAll();
		}
	}
}
