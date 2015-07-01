using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class UserDelegateRepositroy : EfRepository<UserDelegate, int>, IUserDelegateRepositroy
    {
        public UserDelegateRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
