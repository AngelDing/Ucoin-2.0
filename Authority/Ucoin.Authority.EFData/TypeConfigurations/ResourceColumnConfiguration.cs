using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ResourceColumnConfiguration : EntityTypeConfiguration<ResourceColumn>
    {
        public ResourceColumnConfiguration()
        {
            this.ToTable("ResourceColumn");
            this.HasKey(p => p.Id);
        }
    }
}
