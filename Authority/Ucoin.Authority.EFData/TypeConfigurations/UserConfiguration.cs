
using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.ToTable("User");
            this.HasKey(c => c.Id);
            this.HasMany(p => p.Groups)
                .WithMany(p => p.Users)
                .Map(m =>
                {
                    m.ToTable("UserGroupMapping");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("GroupId");
                });           

            //this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            //this.Property(c => c.UpdatedOnUtc).IsOptional();
        }
    }
}
