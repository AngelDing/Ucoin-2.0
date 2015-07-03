using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.EFData
{
    public class IdentityRepositoryContext : EfRepositoryContext, IIdentityRepositoryContext
    {
        public IdentityRepositoryContext()
            : base(new IdentityDbContext())
        { 
        }
    }
}
