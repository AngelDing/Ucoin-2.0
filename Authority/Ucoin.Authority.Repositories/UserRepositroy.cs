using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class UserRepositroy<TUser, TRole> : UserStore<TUser, TRole, int, UserLogin, UserRole, UserClaim>
        where TUser : User, new()
        where TRole : Role, new()
    {
        public UserRepositroy(DbContext dbContext)
            : base(dbContext)
        {
        }
    }

    public class UserRepositroy : UserRepositroy<User, Role>, IUserRepositroy
    {
        public UserRepositroy(IIdentityRepositoryContext context)
            : base(context.DbContext)
        {
        }
    }
}
