using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            this.ToTable("Group");
        }
    }
}
