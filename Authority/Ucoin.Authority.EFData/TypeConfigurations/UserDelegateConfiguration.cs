using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserDelegateConfiguration : EntityTypeConfiguration<UserDelegate>
    {
        public UserDelegateConfiguration()
        {
            this.ToTable("UserDelegate");
        }
    }
}
