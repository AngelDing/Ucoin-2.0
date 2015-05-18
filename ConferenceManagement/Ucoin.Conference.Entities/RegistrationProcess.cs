using System;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;
using Ucoin.Framework.Utils;

namespace Ucoin.Conference.Entities
{
    public class RegistrationProcess : EfEntity<Guid>, IAggregateRoot<Guid>
    {
        public RegistrationProcess()
        {
            this.Id = GuidHelper.NewSequentialId();
        }

        public bool Completed { get; set; }

        public Guid ConferenceId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ReservationId { get; set; }

        public Guid SeatReservationCommandId { get; set; }

        // feels awkward and possibly disrupting to store these properties here. Would it be better if instead of using 
        // current state values, we use event sourcing?
        public DateTime? ReservationAutoExpiration { get; set; }

        public Guid ExpirationCommandId { get; set; }

        public int StateValue { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public byte[] TimeStamp { get; private set; }
    }
}
