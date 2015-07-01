using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ButtonRepository : EfRepository<Button, int>, IButtonRepository
    {
        public ButtonRepository(IAuthorityRepositoryContext context)
            : base(context)
        { 
        }
    }
}
