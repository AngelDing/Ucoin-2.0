using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Ucoin.Framework.EFRepository;

namespace Ucoin.Framework.Test
{
    public class EFTestContext : BaseCustomDbContext
    {      
        public EFTestContext()
            : base(ConstHelper.EFTestDBName)
        {
            this.LogChangesDuringSave = true;
        }

        public DbSet<EFCustomer> Customers
        {
            get { return Set<EFCustomer>(); }
        }

        public DbSet<EFNote> Notes
        {
            get { return Set<EFNote>(); }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            modelBuilder.Entity<EFCustomer>().HasKey(p => p.Id);
            modelBuilder.Entity<EFCustomer>().Property(
                p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<EFCustomer>().Property(t => t.Address.City).HasColumnName("City");
            modelBuilder.Entity<EFCustomer>().Property(t => t.Address.Country).HasColumnName("Country");

            modelBuilder.Entity<EFNote>().HasRequired(c => c.Customer)
              .WithMany(t => t.Notes).HasForeignKey(p => p.CustomerId);  //Map(m => m.MapKey("CustomerId"));

            modelBuilder.Entity<EFNote>().HasKey(p => p.Id);
     
            base.OnModelCreating(modelBuilder);
        }
    }
}
