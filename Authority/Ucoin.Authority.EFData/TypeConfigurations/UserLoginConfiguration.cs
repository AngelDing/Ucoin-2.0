using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserLoginConfiguration: EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfiguration()
        {
            this.ToTable("UserLogin");
            this.HasKey(p => p.Id);
        }
    }
}
