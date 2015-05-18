
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;

    /// <summary>
    /// Adds seats to an existing seat type.
    /// </summary>
    public class AddSeats : SeatsAvailabilityCommand
    {
        /// <summary>
        /// Gets or sets the type of the seat.
        /// </summary>
        public Guid SeatType { get; set; }

        /// <summary>
        /// Gets or sets the quantity of seats added.
        /// </summary>
        public int Quantity { get; set; }
    }
}
