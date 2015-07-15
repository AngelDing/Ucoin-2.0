using Microsoft.AspNet.Identity.EntityFramework;
using Ucoin.Framework.Entities;

namespace Ucoin.Authority.Entities
{
    public class UserLogin : IdentityUserLogin<int>, IEntity<int>
    {
        public int Id { get; set; }
    }
}
