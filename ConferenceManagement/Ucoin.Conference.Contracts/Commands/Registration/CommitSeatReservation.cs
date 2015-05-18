
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;

    public class CommitSeatReservation : SeatsAvailabilityCommand
    {
        public Guid ReservationId { get; set; }
    }
}
