
namespace Ucoin.Conference.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Conference.Contracts;

    public interface IPricingService
    {
        OrderTotal CalculateTotal(Guid conferenceId, ICollection<SeatQuantity> seatItems);
    }
}