using System.Data.Entity;
using Ucoin.Conference.Entities;

namespace Ucoin.Conference.EfData
{
    public class ConferenceContext : DbContext
    {
        public const string SchemaName = "ConferenceManagement";

        public ConferenceContext()
            : base("ConferenceManagement")
        {
        }

        public ConferenceContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual DbSet<ConferenceInfo> Conferences { get; set; }

        public virtual DbSet<SeatType> Seats { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConferenceInfo>().ToTable("Conferences", SchemaName);
            modelBuilder.Entity<ConferenceInfo>().HasMany(x => x.Seats).WithRequired();
            modelBuilder.Entity<SeatType>().ToTable("SeatTypes", SchemaName);
            modelBuilder.Entity<Order>().ToTable("Orders", SchemaName);
            modelBuilder.Entity<OrderSeat>().ToTable("OrderSeats", SchemaName);
            modelBuilder.Entity<OrderSeat>().HasKey(seat => new { seat.OrderId, seat.Position });
        }
    }
}
