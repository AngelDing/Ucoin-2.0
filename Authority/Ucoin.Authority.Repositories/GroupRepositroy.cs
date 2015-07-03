using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class GroupRepositroy : EfRepository<Group, int>, IGroupRepositroy
    {
        public GroupRepositroy(IIdentityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
