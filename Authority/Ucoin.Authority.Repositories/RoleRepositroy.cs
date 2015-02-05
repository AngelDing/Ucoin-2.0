using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class RoleRepositroy : EfRepository<Role, int>, IRoleRepositroy
    {
        public RoleRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
