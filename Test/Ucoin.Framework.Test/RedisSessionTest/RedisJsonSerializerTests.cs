using Moq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ucoin.Framework.RedisSession;
using Xunit;

namespace Ucoin.Framework.Test.RedisSession
{
    public class RedisJsonSerializerTests
    {
        private RedisJsonSerializer srsly;

        public RedisJsonSerializerTests()
        {
            this.srsly = new RedisJsonSerializer();
        }

        [Fact]
        public void BasicCorrectnessTest()
        {
            // basic test of a string
            string testString = "foo bar baz";
            string testSerialized = this.srsly.SerializeOne(testString);

            Assert.Equal(testString, (string)this.srsly.DeserializeOne(testSerialized));

            // basic test of an int
            int testInt = 153;
            string testIntSrlzed = this.srsly.SerializeOne(testInt);

            Assert.Equal(testInt, (int)this.srsly.DeserializeOne(testIntSrlzed));

            // basic test of a long
            long testLong = 123456L;
            string testLongSrlzed = this.srsly.SerializeOne(testLong);

            Assert.Equal(testLong, (long)this.srsly.DeserializeOne(testLongSrlzed));

            // basic test of a long
            double testDouble = 123456.756D;
            string testDoubleSrlzed = this.srsly.SerializeOne(testDouble);

            Assert.Equal(testDouble, (double)this.srsly.DeserializeOne(testDoubleSrlzed));

            // basic test of a decimal
            decimal testFloat = 1234.8564M;
            string testFloatSrlzed = this.srsly.SerializeOne(testFloat);

            Assert.Equal(testFloat, (decimal)this.srsly.DeserializeOne(testFloatSrlzed));

            // basic test of a long
            int[] testIntArr = new int[] { 1, 2, 3 };
            string testIntArrSrlzed = this.srsly.SerializeOne(testIntArr);

            Assert.Equal(testIntArr, (int[])this.srsly.DeserializeOne(testIntArrSrlzed));

            // basic test of a long
            string[] testStringArr = new string[] { "a", "b", "c" };
            string testStringArrSrlzed = this.srsly.SerializeOne(testStringArr);

            Assert.Equal(testStringArr, (string[])this.srsly.DeserializeOne(testStringArrSrlzed));                     
        
            // basic class serialization
            TestSerializableClass tClass = new TestSerializableClass() 
            { 
                Prop1 = "first",
                Prop2 = 2,
                Prop3 = null
            };

            string tClassSrlzed = this.srsly.SerializeOne(tClass);

            TestSerializableClass dsrlzdTClass = this.srsly.DeserializeOne(tClassSrlzed) as TestSerializableClass;

            Assert.True(tClass.Equals(dsrlzdTClass));
            Assert.Throws<InvalidCastException>(() => 
            {
                var x = (TestSubclass)this.srsly.DeserializeOne(tClassSrlzed);
            });

            // test deserialize type correctness
            TestSubclass subTClass = new TestSubclass()
            {
                Prop1 = "second",
                Prop2 = 42,
                Prop3 = new List<string> 
                { 
                    "a",
                    "b"
                }
            };

            string subClassSrlzed = this.srsly.SerializeOne(subTClass);

            TestSerializableClass dsrlzdSubTClass = this.srsly.DeserializeOne(subClassSrlzed) as TestSubclass;

            Assert.True(dsrlzdSubTClass.Equals(subTClass));
        }     
    }

    [Serializable]
    public class TestSerializableClass
    {
        public string Prop1 { get; set; }

        public int Prop2 { get; set; }

        public List<string> Prop3 { get; set; }

        [IgnoreDataMember]
        public long IgnoredProp { get; set; }

        public override bool Equals(object obj)
        {
            TestSerializableClass other = obj as TestSerializableClass;
            if (other != null)
            {
                return
                    this.Prop1 == other.Prop1 &&
                    this.Prop2 == other.Prop2 &&
                    this.Prop3Equal(other.Prop3);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private bool Prop3Equal(List<string> otherList)
        {
            if (this.Prop3 == null && otherList == null)
            {
                return true;
            }
            else if (this.Prop3 != null && otherList != null
                && this.Prop3.Count == otherList.Count)
            {
                bool allElementsEqual = true;

                for (int i = 0; i < this.Prop3.Count; i++)
                {
                    if (this.Prop3[i] != otherList[i])
                    {
                        allElementsEqual = false;
                        break;
                    }
                }

                return allElementsEqual;
            }

            return false;
        }
    }

    public class TestSubclass : TestSerializableClass
    {
    }
}
