using Moq;
using System;
using System.Collections.Generic;
using Ucoin.Framework.RedisSession;
using Xunit;

namespace Ucoin.Framework.Test.RedisSession
{
    public class SingleThreadedTests
    {
        private RedisSessionStateItemCollection items;
        private RedisJsonSerializer srsly;
        
        public SingleThreadedTests()
        {
            srsly = new RedisJsonSerializer();

            this.items = new RedisSessionStateItemCollection(
                new Dictionary<string, string> { 
                    { "a", srsly.SerializeOne("x")},
                    { "b", srsly.SerializeOne("y")},
                    { "c", srsly.SerializeOne("z")}
                },
                "fakeCollection");
        }

        [Fact]
        public void RedisItemsCollectionConstructorTest()
        {
            Assert.Equal("x", (string)this.items["a"]);
            Assert.Equal("y", (string)this.items["b"]);
            Assert.Equal("z", (string)this.items["c"]);
        }

        [Fact]
        public void RedisItemsCollectionAddTest()
        {
            this.items["something"] = "a thing";
            this.items["foo"] = "bar";
            this.items["lucas"] = "uses venmo";

            Assert.Equal("a thing", this.items["something"]);
            Assert.Equal("bar", this.items["foo"]);
            Assert.Equal("uses venmo", this.items["lucas"]);

            Assert.Throws<NotImplementedException>(
                () => {
                    var x = this.items[3];
                });
        }

        [Fact]
        public void RedisItemsCollectionRemoveTest()
        {
            Assert.Equal("x", (string)this.items["a"]);
            Assert.Equal("y", (string)this.items["b"]);
            Assert.Equal("z", (string)this.items["c"]);

            this.items.Remove("a");

            Assert.Null(this.items["a"]);

            this.items["something"] = "a thing";
            this.items["foo"] = "bar";
            this.items["lucas"] = "uses venmo";
            
            Assert.Equal("y", (string)this.items["b"]);
            Assert.Equal("z", (string)this.items["c"]);
            Assert.Equal("a thing", this.items["something"]);
            Assert.Equal("bar", this.items["foo"]);
            Assert.Equal("uses venmo", this.items["lucas"]);

            this.items.Remove("foo");

            Assert.Null(this.items["foo"]);

            Assert.Equal("y", (string)this.items["b"]);
            Assert.Equal("z", (string)this.items["c"]);
            Assert.Equal("a thing", this.items["something"]);
            Assert.Equal("uses venmo", this.items["lucas"]);

            Assert.Equal(4, this.items.Count);

            this.items["empty"] = null;

            Assert.Equal(4, this.items.Count);
        }

        [Fact]
        public void RedisItemsEnumeratorTest()
        {
            foreach(KeyValuePair<string, object> val in this.items)
            {
                Assert.Contains(val.Value, new string[] { "x", "y", "z" });
            }

            this.items["something"] = "a thing";
            this.items["foo"] = "bar";
            this.items["lucas"] = "uses venmo";

            foreach (KeyValuePair<string, object> val in this.items)
            {
                Assert.Contains(val.Value, new string[] { "x", "y", "z", "a thing", "bar", "uses venmo" });
            }
        }

        [Fact]
        public void RedisItemsChangedObjsEnumeratorTest()
        {
            List<KeyValuePair<string, string>> changedObjs = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                changedObjs.Add(val);
            }

            Assert.Equal(0, changedObjs.Count);

            this.items["something"] = "a thing";
            this.items["foo"] = "bar";
            this.items["lucas"] = "uses venmo";

            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                changedObjs.Add(val);
            }

            Assert.Equal(3, changedObjs.Count);
            
            foreach(KeyValuePair<string, string> val in changedObjs)
            {
                Assert.Contains(
                    this.srsly.DeserializeOne(val.Value), 
                    new string[] { "a thing", "bar", "uses venmo" });
            }

            this.items["a"] = "not x";

            changedObjs.Clear();
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                changedObjs.Add(val);
            }

            // since we got all the new changed objects in the previous call to GetChangedObjectsEnumerator,
            //      this call should only return "a", "not x"
            Assert.Equal(1, changedObjs.Count);
            Assert.Equal(changedObjs[0].Key, "a");
            Assert.Equal( 
                this.srsly.DeserializeOne(changedObjs[0].Value), 
                "not x");
        }

        [Fact]
        public void AddRemoveAndGetEnumeratorTest()
        {
            // test that nothing has changed since the stuff we added was in the constructor,
            //      so in real usage that would be coming from Redis and thus be the default state
            int numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
            }

            Assert.Equal(0, numChanged);

            // test assigning a value to something new
            this.items["a"] = "not x";

            numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
                Assert.Equal(
                    val.Key, 
                    "a");
                Assert.Equal(
                    this.srsly.DeserializeOne(val.Value), 
                    "not x");
            }

            Assert.Equal(1, numChanged);

            // test assigning a value to something else then assigning it back. In such a case,
            //      we don't want it to come back from the enumerator because it has not really
            //      changed from the initial state so why bother redis about it?
            this.items["a"] = "x";
            this.items["a"] = "not x";

            numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
            }

            Assert.Equal(0, numChanged);

            // test creating a new value and then removing it will do nothing
            this.items["new"] = "m";
            this.items["new"] = null;

            numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
            }

            Assert.Equal(0, numChanged);

            // test creating a new value, then getting the changed objects enumerator (which resets
            //      the initial state) then removing the new value then getting the changed objects
            //      again results in two enumerators that return 1 element each

            this.items["new"] = "m";
            numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
                Assert.Equal(
                    val.Key, 
                    "new");
                Assert.Equal(
                    this.srsly.DeserializeOne(val.Value), 
                    "m");
            }

            Assert.Equal(1, numChanged);

            this.items["new"] = null;

            numChanged = 0;
            foreach (KeyValuePair<string, string> val in this.items.GetChangedObjectsEnumerator())
            {
                numChanged++;
                Assert.Equal(
                    val.Key, 
                    "new");
                Assert.Null(val.Value);
            }

            Assert.Equal(1, numChanged);

            

        }

        [Fact]
        public void AddRemoveAndDirtyCheckReferenceTypesTest()
        {
            // add a list to session
            this.items["refType"] = new List<string>();
            // get a reference to it
            List<string> myList = this.items["refType"] as List<string>;

            // alternatively, make a list
            List<int> myOtherList = new List<int>();
            // add it to session
            this.items["otherRefType"] = myOtherList;

            bool listCameBack = false;
            bool otherListCameBack = false;
            // test that these come back from the enumerator
            foreach(KeyValuePair<string, string> changed in this.items.GetChangedObjectsEnumerator())
            {
                if(changed.Key == "refType" &&
                    changed.Value == srsly.SerializeOne(myList))
                {
                    listCameBack = true;
                }
                else if (changed.Key == "otherRefType" &&
                    changed.Value == srsly.SerializeOne(myOtherList))
                {
                    otherListCameBack = true;
                }
            }

            Assert.True(listCameBack, "failed to return string list");
            Assert.True(otherListCameBack, "failed to return int list");

            // test that if we get the changed objects again, they won't come back
            listCameBack = false;
            otherListCameBack = false;

            foreach (KeyValuePair<string, string> changed in this.items.GetChangedObjectsEnumerator())
            {
                if(changed.Key == "refType")
                {
                    listCameBack = true;
                }
                else if(changed.Key == "otherRefType")
                {
                    otherListCameBack = true;
                }
            }

            Assert.False(listCameBack, "incorrectly returned string list");
            Assert.False(otherListCameBack, "incorrectly returned int list");


            // now let's modify a list
            myOtherList.Add(1);

            otherListCameBack = false;
            foreach (KeyValuePair<string, string> changed in this.items.GetChangedObjectsEnumerator())
            {
                if(changed.Key == "otherRefType" && 
                    changed.Value == srsly.SerializeOne(myOtherList))
                {
                    otherListCameBack = true;
                }
            }

            Assert.True(otherListCameBack, "list that was modified not returned when it should have been");

            // ok, let's see if modifying the list, then undoing that modification results
            //      in it being returned (it shouldn't)

            myList.Add("a");
            myList.Clear();

            listCameBack = false;
            foreach (KeyValuePair<string, string> changed in this.items.GetChangedObjectsEnumerator())
            {
                if (changed.Key == "refType" && 
                    changed.Value == srsly.SerializeOne(myList))
                {
                    listCameBack = true;
                }
            }

            Assert.False(listCameBack, "list that was modified then reset should not come back");
        }
    }
}
