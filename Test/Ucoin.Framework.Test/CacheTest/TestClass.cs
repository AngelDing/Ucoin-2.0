using System;

namespace Ucoin.Framework.Test.Caching
{
    [Serializable]
    public class TestClass<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }
    }
}