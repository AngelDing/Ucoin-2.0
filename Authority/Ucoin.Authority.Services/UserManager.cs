using Microsoft.AspNet.Identity;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Authority.IServices;

namespace Ucoin.Authority.Services
{
    public class UserManager : UserManager<User, int>
    {
        private IUserRepositroy userRepositroy;
        public UserManager(IUserRepositroy userRepositroy) : base(userRepositroy)
        {
            this.userRepositroy = userRepositroy;
        }
    }
}
