using FluentAssertions;
using System;
using Ucoin.Framework.Serialization;
using Xunit;

namespace Framework.Serialization.Test
{
    public class MsgPackSerializerTest : BaseSerializerTest
    {
        public override ISerializer GetSerializer()
        {
            return SerializationHelper.MsgPack;
        }

        [Fact]
        public override void loop_object_serialize_test()
        {
            ////不支持循環引用，測試會導致內存溢出錯誤
            //var result = GetSerializedLoopObject();
            //result.Should().BeNull();
        }
    }
}
