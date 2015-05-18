using System.Data.Entity;
using Ucoin.Framework.SqlDb.BlobStorage;
using Ucoin.Framework.SqlDb.EventSourcing;
using Ucoin.Framework.SqlDb.MessageLog;

namespace Ucoin.Conference.Processor
{
    internal static class DatabaseSetup
    {
        public static void Initialize()
        {
            //Database.SetInitializer<ConferenceRegistrationDbContext>(null);
            //Database.SetInitializer<RegistrationProcessManagerDbContext>(null);
            Database.SetInitializer<EventStoreDbContext>(null);
            Database.SetInitializer<MessageLogDbContext>(null);
            Database.SetInitializer<BlobStorageDbContext>(null);

            //Database.SetInitializer<PaymentsDbContext>(null);
            //Database.SetInitializer<PaymentsReadDbContext>(null);
        }
    }
}
