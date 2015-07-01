using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ResourceConfiguration : EntityTypeConfiguration<Resource>
    {
        public ResourceConfiguration()
        {
            this.ToTable("Resource");
            this.HasMany(p => p.ResourceButtons).WithRequired(p => p.Resource).HasForeignKey(p => p.ResourceId);
            this.HasMany(p => p.ResourceColumns).WithRequired(p => p.Resource).HasForeignKey(p => p.ResourceId);
        }
    }
}
