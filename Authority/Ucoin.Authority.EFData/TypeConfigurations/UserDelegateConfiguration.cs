using System.Data.Entity.ModelConfiguration;
using Ucoin.Authority.Entities;

namespace Ucoin.Authority.EFData.TypeConfigurations
{
    public class UserDelegateConfiguration : EntityTypeConfiguration<UserDelegate>
    {
        public UserDelegateConfiguration()
        {
            this.ToTable("UserDelegate");
            this.Property(p => p.DateRange.StartDateTime).HasColumnName("StartDateTime");
            this.Property(p => p.DateRange.EndDateTime).HasColumnName("EndDateTime"); 
            //this.HasRequired(p => p.Mandator).WithMany(p => p.UserDelegates).HasForeignKey(p => p.MandatorId);
            //this.HasRequired(p => p.Mandatary).WithMany(p => p.UserDelegates).HasForeignKey(p => p.MandataryId);
            //this.HasRequired(p => p.Mandatary).WithMany(p => p.UserDelegates).HasForeignKey(p => p.MandataryId);
        }
    }
}
