using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.EFRepository;
using Ucoin.Framework.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class RoleRepositroy : EFRepository<Role, int>, IRoleRepositroy
    {
        public RoleRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
