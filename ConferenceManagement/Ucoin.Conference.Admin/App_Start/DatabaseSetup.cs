
namespace Ucoin.Conference.Admin
{
    using System.Data.Entity;

    /// <summary>
    /// Initializes the EF infrastructure.
    /// </summary>
    internal static class DatabaseSetup
    {
        public static void Initialize()
        {
            //Database.DefaultConnectionFactory = new ServiceConfigurationSettingConnectionFactory(Database.DefaultConnectionFactory);

            //Database.SetInitializer<ConferenceContext>(null);
        }
    }
}