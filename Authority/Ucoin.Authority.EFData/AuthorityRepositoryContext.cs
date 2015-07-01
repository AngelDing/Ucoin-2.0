using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.EFData
{
    public class AuthorityRepositoryContext : EfRepositoryContext, IAuthorityRepositoryContext
    {
        public AuthorityRepositoryContext()
            : base(new IdentityDbContext())
        { 
        }
    }
}
