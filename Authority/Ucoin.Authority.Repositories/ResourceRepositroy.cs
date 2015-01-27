using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.EFRepository;
using Ucoin.Framework.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ResourceRepositroy : EFRepository<Resource, int>, IResourceRepositroy
    {
        public ResourceRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
