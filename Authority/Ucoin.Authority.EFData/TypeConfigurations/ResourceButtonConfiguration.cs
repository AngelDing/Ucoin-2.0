using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ResourceButtonConfiguration : EntityTypeConfiguration<ResourceButton>
    {
        public ResourceButtonConfiguration()
        {
            this.ToTable("ResourceButton");
            this.HasKey(p => p.Id);
        }
    }
}
