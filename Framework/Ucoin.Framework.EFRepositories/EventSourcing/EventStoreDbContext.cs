namespace Ucoin.Framework.SqlDb.EventSourcing
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Used by <see cref="SqlEventSourcedRepository{T}"/>, and is used only for running the sample application
    /// without the dependency to the Windows Azure Service Bus when using the DebugLocal solution configuration.
    /// </summary>
    public class EventStoreDbContext : DbContext
    {
        public const string SchemaName = "Events";

        public EventStoreDbContext()
            : base("Conference")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventEntity>().ToTable("Events", SchemaName);
        }
    }
}
