namespace Ucoin.Framework.SqlDb.MessageLog
{
    using System.Data.Entity;

    public class MessageLogDbContext : DbContext
    {
        public const string SchemaName = "MessageLog";

        public MessageLogDbContext()
            : base("Conference")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MessageLogEntity>().ToTable("Messages", SchemaName);
        }
    }
}
