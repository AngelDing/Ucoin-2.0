
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;

    public class CancelSeatReservation : SeatsAvailabilityCommand
    {
        public Guid ReservationId { get; set; }
    }
}
