namespace Ucoin.Conference.Contracts.Events.Registration
{
    using Ucoin.Framework.EventSourcing;

    public class OrderRegistrantAssigned : VersionedEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
