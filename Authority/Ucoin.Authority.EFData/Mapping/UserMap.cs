
using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("User");
            this.HasKey(c => c.Id);
            //this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            //this.Property(c => c.UpdatedOnUtc).IsOptional();
        }
    }
}
