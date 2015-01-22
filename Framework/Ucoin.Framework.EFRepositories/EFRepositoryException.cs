using System;

namespace Ucoin.Framework.EFRepository
{
    public class EFRepositoryException : Exception
    {
        public EFRepositoryException(string message)
            : base(message)
        {
        }
    }
}
