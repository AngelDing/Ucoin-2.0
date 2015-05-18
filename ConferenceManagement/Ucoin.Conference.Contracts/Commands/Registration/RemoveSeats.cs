
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;

    public class RemoveSeats : SeatsAvailabilityCommand
    {
        /// <summary>
        /// Gets or sets the type of the seat.
        /// </summary>
        public Guid SeatType { get; set; }

        /// <summary>
        /// Gets or sets the quantity of seats removed.
        /// </summary>
        public int Quantity { get; set; }
    }
}
