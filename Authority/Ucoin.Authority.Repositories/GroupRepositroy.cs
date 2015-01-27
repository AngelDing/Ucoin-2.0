using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.EFRepository;
using Ucoin.Framework.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class GroupRepositroy : EFRepository<Group, int>, IGroupRepositroy
    {
        public GroupRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
