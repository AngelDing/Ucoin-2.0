using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ResourceRepositroy : EfRepository<Resource, int>, IResourceRepositroy
    {
        public ResourceRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
