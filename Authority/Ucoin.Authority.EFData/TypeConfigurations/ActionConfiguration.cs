using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class ActionConfiguration : EntityTypeConfiguration<Action>
    {
        public ActionConfiguration()
        {
            this.ToTable("Action");
            this.HasMany(p => p.ResourceActions).WithRequired(p => p.Button).HasForeignKey(p => p.ActionId);
        }
    }
}
