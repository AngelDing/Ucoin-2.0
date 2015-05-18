using System.Data.Entity;

namespace Ucoin.Conference.EfData
{
    public class DatabaseInitializer : IDatabaseInitializer<ConferenceContext>
    {
        private IDatabaseInitializer<ConferenceContext> innerInitializer;

        public DatabaseInitializer(IDatabaseInitializer<ConferenceContext> innerInitializer)
        {
            this.innerInitializer = innerInitializer;
        }

        public void InitializeDatabase(ConferenceContext context)
        {
            this.innerInitializer.InitializeDatabase(context);

            CreateIndexes(context);

            context.SaveChanges();
        }

        public static void CreateIndexes(DbContext context)
        {
            context.Database.ExecuteSqlCommand(@"
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_RegistrationProcessManager_Completed')
CREATE NONCLUSTERED INDEX IX_RegistrationProcessManager_Completed ON [" + ConferenceContext.RegistrationProcessesSchemaName + @"].[RegistrationProcess]( Completed )
            
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_RegistrationProcessManager_OrderId')
CREATE NONCLUSTERED INDEX IX_RegistrationProcessManager_OrderId ON [" + ConferenceContext.RegistrationProcessesSchemaName + @"].[RegistrationProcess]( OrderId )");

            //IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_RegistrationProcessManager_ReservationId')
            //CREATE NONCLUSTERED INDEX IX_RegistrationProcessManager_ReservationId ON [" + RegistrationProcessDbContext.SchemaName + @"].[RegistrationProcess]( ReservationId )
        }
    }
}