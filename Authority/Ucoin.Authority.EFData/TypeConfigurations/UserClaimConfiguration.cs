using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfiguration()
        {
            this.ToTable("UserClaim");
        }
    }
}
