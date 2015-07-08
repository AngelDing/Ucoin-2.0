using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ActionRepository : EfRepository<Action, int>, IActionRepository
    {
        public ActionRepository(IIdentityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
