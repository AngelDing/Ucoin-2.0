using System;

namespace Ucoin.Framework
{
    public class UcoinException : Exception
    {
        public UcoinException(string message)
            : base(message)
        {
        }
    }
}
