
namespace Ucoin.Registration.Contracts
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.EventSourcing;

    public class OrderPlaced : VersionedEvent
    {
        public Guid ConferenceId { get; set; }

        public IEnumerable<SeatQuantity> Seats { get; set; }

        /// <summary>
        /// The expected expiration time if the reservation is not explicitly confirmed later.
        /// </summary>
        public DateTime ReservationAutoExpiration { get; set; }

        public string AccessCode { get; set; }
    }
}
