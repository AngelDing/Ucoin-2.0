
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using System.Collections.Generic;

    public class MakeSeatReservation : SeatsAvailabilityCommand
    {
        public MakeSeatReservation()
        {
            this.Seats = new List<SeatQuantity>();
        }

        public Guid ReservationId { get; set; }
        public List<SeatQuantity> Seats { get; set; }
    }
}
