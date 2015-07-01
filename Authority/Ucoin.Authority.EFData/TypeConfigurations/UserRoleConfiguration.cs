
using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;
namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            this.ToTable("UserRoleMapping");
        }
    }
}
