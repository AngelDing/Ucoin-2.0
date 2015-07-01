using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ButtonConfiguration : EntityTypeConfiguration<Button>
    {
        public ButtonConfiguration()
        {
            this.ToTable("Button");
            this.HasMany(p => p.ResourceButtons).WithRequired(p => p.Button).HasForeignKey(p => p.ButtonId);
        }
    }
}
