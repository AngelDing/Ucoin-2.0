using System;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public class EfRepositoryException : Exception
    {
        public EfRepositoryException(string message)
            : base(message)
        {
        }
    }
}
