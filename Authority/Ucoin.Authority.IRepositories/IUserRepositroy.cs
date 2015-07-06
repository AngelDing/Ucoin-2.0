using Ucoin.Authority.Entities;
using Ucoin.Framework.SqlDb.Repositories;
using Microsoft.AspNet.Identity;

namespace Ucoin.Authority.IRepositories
{
    public interface IUserRepositroy : IUserStore<User, int>
    {
    }
}
