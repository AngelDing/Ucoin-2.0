using System.Data.Entity;
using Ucoin.Conference.Entities;
using Ucoin.Conference.Entities.Payments;
using Ucoin.Conference.Entities.Registration;
using Ucoin.Framework.SqlDb.Processes;

namespace Ucoin.Conference.EfData
{
    public class ConferenceContext : DbContext
    {
        public const string SchemaName = "ConferenceManagement";
        public const string RegistrationProcessesSchemaName = "ConferenceRegistrationProcesses";

        public ConferenceContext()
            : base("Conference")
        {
        }

        public ConferenceContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
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
            modelBuilder.Entity<ConferenceInfo>().HasMany(x => x.Seats).WithRequired();
            modelBuilder.Entity<ConferenceInfo>().Property(t => t.BookableDateRange.StartDateTime).HasColumnName("StartDate");
            modelBuilder.Entity<ConferenceInfo>().Property(t => t.BookableDateRange.EndDateTime).HasColumnName("EndDate");
            modelBuilder.Entity<SeatType>().ToTable("SeatTypes", SchemaName);
            modelBuilder.Entity<Order>().ToTable("Orders", SchemaName);
            modelBuilder.Entity<OrderSeat>().ToTable("OrderSeats", SchemaName);
            modelBuilder.Entity<OrderSeat>().HasKey(seat => new { seat.OrderId, seat.Position });

            modelBuilder.Entity<Payment>().ToTable("Payment", "ConferencePayments");
            modelBuilder.Entity<PaymentItem>().ToTable("PaymentItem", "ConferencePayments");
            modelBuilder.Entity<RegistrationProcess>().ToTable("RegistrationProcess", SchemaName);
            modelBuilder.Entity<UndispatchedMessages>().ToTable("UndispatchedMessages", SchemaName);
        }
    }
}
