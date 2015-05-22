using System.Data.Entity;
using Ucoin.Conference.Entities;
using Ucoin.Framework.SqlDb;
using Ucoin.Framework.SqlDb.Processes;

namespace Ucoin.Conference.EfData
{
    public class ConferenceContext : BaseCustomDbContext
    {
        public const string SchemaName = "ConferenceManagement";
        public const string RegistrationProcessesSchemaName = "ConferenceRegistrationProcesses";

        public ConferenceContext()
            : base("Conference")
        {
        }

        public virtual DbSet<ConferenceInfo> Conferences { get; set; }

        public virtual DbSet<SeatType> Seats { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public DbSet<Payment> ThirdPartyProcessorPayments { get; set; }

        // Define the available entity sets for the database.
        public DbSet<RegistrationProcess> RegistrationProcesses { get; set; }

        // Table for pending undispatched messages associated with a process manager.
        public DbSet<UndispatchedMessages> UndispatchedMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConferenceInfo>().ToTable("Conferences", SchemaName);
            modelBuilder.Entity<ConferenceInfo>()
                .HasMany(c => c.Seats)
                .WithRequired(c => c.ConferenceInfo)
                .HasForeignKey(c => c.ConferenceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SeatType>().ToTable("SeatTypes", SchemaName);
            modelBuilder.Entity<SeatType>().HasKey(p => p.Id);
            //modelBuilder.Entity<SeatType>().HasRequired(c => c.ConferenceInfo)
            //  .WithMany(t => t.Seats).HasForeignKey(p => p.ConferenceId);            
            
            modelBuilder.Entity<Order>().ToTable("Orders", SchemaName);
            modelBuilder.Entity<OrderSeat>().ToTable("OrderSeats", SchemaName);
            modelBuilder.Entity<OrderSeat>().HasKey(seat => new { seat.OrderId, seat.Position });

            modelBuilder.Entity<Payment>().ToTable("Payment", "ConferencePayments");
            modelBuilder.Entity<PaymentItem>().ToTable("PaymentItem", "ConferencePayments");
            modelBuilder.Entity<RegistrationProcess>().ToTable("RegistrationProcess", RegistrationProcessesSchemaName);
            modelBuilder.Entity<UndispatchedMessages>().ToTable("UndispatchedMessages", RegistrationProcessesSchemaName);
        }
    }
}
