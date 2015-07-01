using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class PermissionRepository : EfRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(IAuthorityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
