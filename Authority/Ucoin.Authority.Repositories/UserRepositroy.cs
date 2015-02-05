using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class UserRepositroy : EfRepository<User, int>, IUserRepositroy
    {
        public UserRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
