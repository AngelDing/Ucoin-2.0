using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.ToTable("Role");
            this.HasMany(p => p.Permissions)
                .WithMany(p => p.Roles)
                .Map(m =>
                {
                    m.ToTable("RolePermissionMapping");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("PermissionId");
                });
        }
    }
}
