
namespace Ucoin.Conference.Admin
{
    using System.Data.Entity;
    using Ucoin.Conference.EfData;

    /// <summary>
    /// Initializes the EF infrastructure.
    /// </summary>
    internal static class DatabaseSetup
    {
        public static void Initialize()
        {
            Database.SetInitializer<ConferenceContext>(null);
        }
    }
}