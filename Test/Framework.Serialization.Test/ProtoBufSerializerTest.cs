using FluentAssertions;
using System;
using Ucoin.Framework.Serialization;
using Xunit;

namespace Framework.Serialization.Test
{
    public class ProtoBufSerializerTest : BaseSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.ProtoBuf;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().BeNull();
        }
    }
}
