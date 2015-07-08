using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ResourceActionConfiguration : EntityTypeConfiguration<ResourceAction>
    {
        public ResourceActionConfiguration()
        {
            this.ToTable("ResourceAction");
            this.HasKey(p => p.Id);
        }
    }
}
