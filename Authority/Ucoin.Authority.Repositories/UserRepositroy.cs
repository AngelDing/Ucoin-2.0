using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.EFRepository;
using Ucoin.Framework.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class UserRepositroy : EFRepository<User, int>, IUserRepositroy
    {
        public UserRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
