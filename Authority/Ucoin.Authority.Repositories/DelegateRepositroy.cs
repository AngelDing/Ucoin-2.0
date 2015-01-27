using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.EFRepository;
using Ucoin.Framework.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class DelegateRepositroy : EFRepository<Delegate, int>, IDelegateRepositroy
    {
        public DelegateRepositroy(IAuthorityRepositoryContext context)
            : base(context)
        {
        }
    }
}
