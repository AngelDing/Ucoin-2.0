using Ucoin.Framework.EFRepository;

namespace Ucoin.Authority.EFData
{
    public class AuthorityRepositoryContext : EFRepositoryContext, IAuthorityRepositoryContext
    {
        public AuthorityRepositoryContext()
            : base(new AuthorityDbContext())
        { 
        }
    }
}
