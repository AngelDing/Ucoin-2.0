using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class DelegateRepositroy : EfRepository<Delegate, int>, IDelegateRepositroy
    {
        public DelegateRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
