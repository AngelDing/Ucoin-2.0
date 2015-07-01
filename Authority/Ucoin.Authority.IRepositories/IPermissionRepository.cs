using Ucoin.Authority.Entities;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.IRepositories
{
    public interface IPermissionRepository : IEfRepository<Permission, int>
    {
    }
}
