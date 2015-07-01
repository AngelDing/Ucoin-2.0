using Microsoft.AspNet.Identity.EntityFramework;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class UserRepositroy : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>, IUserRepositroy
    {
        public UserRepositroy(IAuthorityRepositoryContext context)
            : base(context.DbContext)
        {
        }
    }
}
