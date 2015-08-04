using FluentAssertions;
using System;
using Ucoin.Framework.Serialization;
using Xunit;

namespace Framework.Serialization.Test
{
    public class XmlSerializerTest : BaseSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Xml;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().BeNull();
        }

        [Fact]
        public override void complex_object_deserialize_test()
        {
            var serializedObj = GetSerializedComplexObject();
            var result = Serializer.Deserialize<ComplexObject>(serializedObj);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.ListObjects.Count.Should().Be(3);
            result.OrderItem.Price.Should().Be(20);
            result.TimeSpan.Should().Be(new TimeSpan());
        }
    }
}
