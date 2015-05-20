
namespace Ucoin.Conference.Domain.Services
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Globalization;
    using System.Collections.Generic;
    using Ucoin.Conference.Contracts;
    using Ucoin.Conference.IRepositories;
    using Ucoin.Framework.Utils;    

    public class PricingService : IPricingService
    {
        private readonly ISeatTypeViewRepository repository;

        public PricingService(ISeatTypeViewRepository repository)
        {
            if (repository == null)
            {
                GuardHelper.ArgumentNotNull(() => repository);
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public OrderTotal CalculateTotal(Guid conferenceId, ICollection<SeatQuantity> seatItems)
        {
            var seatTypes = this.repository.GetPublishedSeatTypes(conferenceId);
            var lineItems = new List<OrderLine>();
            foreach (var item in seatItems)
            {
                var seatType = seatTypes.FirstOrDefault(x => x.SeatTypeId == item.SeatType);
                if (seatType == null)
                {
                    throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Invalid seat type ID '{0}' for conference with ID '{1}'", item.SeatType, conferenceId));
                }

                lineItems.Add(new SeatOrderLine { SeatType = item.SeatType, Quantity = item.Quantity, UnitPrice = seatType.Price, LineTotal = Math.Round(seatType.Price * item.Quantity, 2) });
            }

            return new OrderTotal
                       {
                           Total = lineItems.Sum(x => x.LineTotal),
                           Lines = lineItems
                       };
        }
    }
}