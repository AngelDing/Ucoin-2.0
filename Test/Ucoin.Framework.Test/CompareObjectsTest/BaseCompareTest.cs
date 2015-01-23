using System;
using System.IO;
using Ucoin.Framework.CompareObjects;

namespace Ucoin.Framework.Test
{
    public class BaseCompareTest : IDisposable
    {
        public CompareLogic CompareLogic { get; private set; }
        public BaseCompareTest()
        {
            CompareLogic = new CompareLogic();
        }

        public void Dispose()
        {
            CompareLogic = null;
        }
    }
}
