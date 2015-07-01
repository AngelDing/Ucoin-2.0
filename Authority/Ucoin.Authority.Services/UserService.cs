using Microsoft.AspNet.Identity;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Authority.IServices;

namespace Ucoin.Authority.Services
{
    public class UserService : IUserService
    {
        private IUserRepositroy userRepositroy;
        public UserService(IUserRepositroy userRepositroy)
        {
            this.userRepositroy = userRepositroy;
        }
    }
}
