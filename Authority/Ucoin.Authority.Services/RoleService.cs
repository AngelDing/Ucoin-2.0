using Ucoin.Authority.IRepositories;
using Ucoin.Authority.IServices;

namespace Ucoin.Authority.Services
{
    public class RoleService : IRoleService
    {
        private IRoleRepositroy roleRepositroy;
        public RoleService(IRoleRepositroy roleRepositroy)
        {
            this.roleRepositroy = roleRepositroy;
        }
    }
}
