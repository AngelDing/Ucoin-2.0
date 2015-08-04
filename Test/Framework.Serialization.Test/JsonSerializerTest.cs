using FluentAssertions;
using System;
using Ucoin.Framework.Serialization;
using Xunit;

namespace Framework.Serialization.Test
{
    public class JsonSerializerTest : BaseSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Json;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().NotBeNull();
        }
    }
}
