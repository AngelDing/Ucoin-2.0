using Xunit;
using FluentAssertions;
using Microsoft.Xunit;
using Ucoin.Framework.Serialization;

namespace Framework.Serialization.Test
{
    public class BinarySerializerTest : BaseSerializerTest
    {      
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.Binary;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            var result = GetSerializedLoopObject();
            result.Should().NotBeNull();
        }
    }
}
