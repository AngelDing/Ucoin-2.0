using FluentAssertions;
using Microsoft.Xunit;
using System;
using System.Collections.Generic;
using Ucoin.Framework.Serialization;
using Xunit;

namespace Framework.Serialization.Test
{
    public class BaseSerializerTest
    {
        public ISerializer Serializer { get; private set; }

        public BaseSerializerTest()
        {
            //Serializer = GetSerializer();
            Serializer = SerializationHelper.Xml;
        }

        public virtual ISerializer GetSerializer()
        {
            return null;
        }

        public virtual void loop_object_serialize_test()
        { 
        }

        [Fact]
        public virtual void simple_object_serialize_test()
        {
            var result = GetSerializedSimpleObject();
            result.Should().NotBeNull();
        }

        [Fact]
        public virtual void complex_object_serialize_test()
        {
            var result = GetSerializedComplexObject();
            result.Should().NotBeNull();
        }

        [Fact]
        public virtual void complex_object_deserialize_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.ListObjects.Count.Should().Be(3);
            result.OrderItem.Price.Should().Be(20);
            result.TimeSpan.Should().Be(new TimeSpan(1, 1, 1));
        }

        [Benchmark(Iterations = 100)]
        public virtual void serialize_benchmark_test()
        {
            var result = GetSerializedSimpleObject();
        }

        [Benchmark]
        public virtual void deserialize_benchmark_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
        }

        internal object GetSerializedLoopObject()
        {
            var loopObject = CreateLoopObject();
            var result = Serializer.Serialize(loopObject);
            return result;
        }

        internal object GetSerializedSimpleObject()
        {
            var simpleObject = CreateSimpleObject();
            var result = Serializer.Serialize(simpleObject);
            return result;
        }

        internal object GetSerializedComplexObject()
        {
            var simpleObject = CreateComplexObject();
            var result = Serializer.Serialize(simpleObject);
            return result;
        }

        private object CreateComplexObject()
        {
            var obj = new ComplexObject
            {
                Id = 1,
                Name = "Complex Object",
                Price = 1000,
                TimeSpan = new TimeSpan(1, 1, 1),
                CreatedDate = DateTime.Now,
                //UpdatedDate = DateTimeOffset.Now.UtcDateTime,
                OrderItem = new OrderItem
                {
                    Id = 1,
                    Name = "Order Item",
                    Price = 20,
                    Qty = 3,
                    SubTotal = 60
                },
                ListObjects = new List<ListObject>
                {
                    new ListObject { Id = 1, Name =  "Item1"},
                    new ListObject { Id = 2, Name =  "Item2"},
                    new ListObject { Id = 3, Name =  "Item3"}
                }
            };

            return obj;
        }

        private object CreateSimpleObject()
        {
            var obj = new SimpleObjects
            {
                Id = 1,
                Name = "Test Name",
                Price = 100,
                CreatedDate = DateTime.Now
            };
            return obj;
        }

        private Book CreateLoopObject()
        {
            var book = new Book
            {
                Name = "平凡世界"
            };
            var autor = new Author
            {
                Name = "路遥"
            };
            book.Author = autor;
            autor.Book = book;
            return book;
        }
    }
}
