using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ApplicationRepository : EfRepository<Application, int>, IApplicationRepository
    {
        public ApplicationRepository(IAuthorityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
