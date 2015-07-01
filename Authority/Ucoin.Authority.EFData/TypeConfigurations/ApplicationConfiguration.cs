using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        public ApplicationConfiguration()
        {
            this.ToTable("Application");
            this.HasMany(p => p.Resources).WithRequired(p => p.Application).HasForeignKey(p => p.AppId);
        }
    }
}
