using Microsoft.AspNet.Identity.EntityFramework;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class RoleRepositroy<TRole> : RoleStore<TRole, int, UserRole>, IRoleRepositroy
        where TRole : Role, new()
    {
        public RoleRepositroy(IAuthorityRepositoryContext context)
            : base(context.DbContext)
        {
        }
    }
}
