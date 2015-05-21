using System.Data.Entity;
using Ucoin.Conference.EfData;
using Ucoin.Framework.SqlDb.BlobStorage;
using Ucoin.Framework.SqlDb.EventSourcing;
using Ucoin.Framework.SqlDb.MessageLog;

namespace Ucoin.Conference.Processor
{
    internal static class DatabaseSetup
    {
        public static void Initialize()
        {
            Database.SetInitializer<EventStoreDbContext>(null);
            Database.SetInitializer<MessageLogDbContext>(null);
            Database.SetInitializer<ConferenceContext>(null);
        }
    }
}
