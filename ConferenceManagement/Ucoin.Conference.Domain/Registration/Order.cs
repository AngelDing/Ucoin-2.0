namespace Ucoin.Conference.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Ucoin.Framework.EventSourcing;
    using Ucoin.Conference.Contracts;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Conference.Domain.Services;
    using Ucoin.Framework.Utils;

    /// <summary>
    /// Represents an order placed by a user.
    /// </summary>
    public class Order : EventSourced
    {
        /// <summary>
        /// Suggest a seats reservation expiration time in case the Order is not completed before this time ellapses.
        /// </summary>
        private static readonly TimeSpan ReservationAutoExpiration = TimeSpan.FromMinutes(15);

        private List<SeatQuantity> seats;
        private bool isConfirmed;
        private Guid conferenceId;

        static Order()
        {
            Mapper.CreateMap<OrderPaymentConfirmed, OrderConfirmed>();
        }

        protected Order(Guid id)
            : base(id)
        {
            base.Handles<OrderPlaced>(this.OnOrderPlaced);
            base.Handles<OrderUpdated>(this.OnOrderUpdated);
            base.Handles<OrderPartiallyReserved>(this.OnOrderPartiallyReserved);
            base.Handles<OrderReservationCompleted>(this.OnOrderReservationCompleted);
            base.Handles<OrderExpired>(this.OnOrderExpired);
            base.Handles<OrderPaymentConfirmed>(e => this.OnOrderConfirmed(Mapper.Map<OrderConfirmed>(e)));
            base.Handles<OrderConfirmed>(this.OnOrderConfirmed);
            base.Handles<OrderRegistrantAssigned>(this.OnOrderRegistrantAssigned);
            base.Handles<OrderTotalsCalculated>(this.OnOrderTotalsCalculated);
        }

        public Order(Guid id, IEnumerable<IVersionedEvent> history)
            : this(id)
        {
            this.LoadFrom(history);
        }

        /// <summary>
        /// Creates a new order with the specified items and id.
        /// </summary>
        /// <remarks>
        /// <param name="id">The identifier.</param>
        /// <param name="conferenceId">The conference that the order applies to.</param>
        /// <param name="items">The desired seats to register to.</param>
        /// <param name="pricingService">Service that calculates the pricing.</param>
        public Order(Guid id, Guid conferenceId, IEnumerable<OrderItem> items, IPricingService pricingService)
            : this(id)
        {
            var all = ConvertItems(items);
            var totals = pricingService.CalculateTotal(conferenceId, all.AsReadOnly());

            this.Update(new OrderPlaced
            {
                ConferenceId = conferenceId,
                Seats = all,
                ReservationAutoExpiration = DateTime.UtcNow.Add(ReservationAutoExpiration),
                AccessCode = HandleGenerator.Generate(6)
            });
            this.Update(new OrderTotalsCalculated { Total = totals.Total, Lines = totals.Lines != null ? totals.Lines.ToArray() : null, IsFreeOfCharge = totals.Total == 0m });
        }

        /// <summary>
        /// Updates the order with the specified items.
        /// </summary>
        /// <param name="items">The desired seats to register to.</param>
        /// <param name="pricingService">Service that calculates the pricing.</param>
        public void UpdateSeats(IEnumerable<OrderItem> items, IPricingService pricingService)
        {
            var all = ConvertItems(items);
            var totals = pricingService.CalculateTotal(this.conferenceId, all.AsReadOnly());

            this.Update(new OrderUpdated { Seats = all });
            this.Update(new OrderTotalsCalculated { Total = totals.Total, Lines = totals.Lines != null ? totals.Lines.ToArray() : null, IsFreeOfCharge = totals.Total == 0m });
        }

        public void MarkAsReserved(IPricingService pricingService, DateTime expirationDate, IEnumerable<SeatQuantity> reservedSeats)
        {
            if (this.isConfirmed)
                throw new InvalidOperationException("Cannot modify a confirmed order.");

            var reserved = reservedSeats.ToList();

            // Is there an order item which didn't get an exact reservation?
            if (this.seats.Any(item => item.Quantity != 0 && !reserved.Any(seat => seat.SeatType == item.SeatType && seat.Quantity == item.Quantity)))
            {
                var totals = pricingService.CalculateTotal(this.conferenceId, reserved.AsReadOnly());

                this.Update(new OrderPartiallyReserved { ReservationExpiration = expirationDate, Seats = reserved.ToArray() });
                this.Update(new OrderTotalsCalculated { Total = totals.Total, Lines = totals.Lines != null ? totals.Lines.ToArray() : null, IsFreeOfCharge = totals.Total == 0m });
            }
            else
            {
                this.Update(new OrderReservationCompleted { ReservationExpiration = expirationDate, Seats = reserved.ToArray() });
            }
        }

        public void Expire()
        {
            if (this.isConfirmed)
                throw new InvalidOperationException("Cannot expire a confirmed order.");

            this.Update(new OrderExpired());
        }

        public void Confirm()
        {
            this.Update(new OrderConfirmed());
        }

        public void AssignRegistrant(string firstName, string lastName, string email)
        {
            this.Update(new OrderRegistrantAssigned
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            });
        }

        public SeatAssignments CreateSeatAssignments()
        {
            if (!this.isConfirmed)
                throw new InvalidOperationException("Cannot create seat assignments for an order that isn't confirmed yet.");

            return new SeatAssignments(this.Id, this.seats.AsReadOnly());
        }

        private static List<SeatQuantity> ConvertItems(IEnumerable<OrderItem> items)
        {
            return items.Select(x => new SeatQuantity(x.SeatType, x.Quantity)).ToList();
        }

        private void OnOrderPlaced(OrderPlaced e)
        {
            this.conferenceId = e.ConferenceId;
            this.seats = e.Seats.ToList();
        }

        private void OnOrderUpdated(OrderUpdated e)
        {
            this.seats = e.Seats.ToList();
        }

        private void OnOrderPartiallyReserved(OrderPartiallyReserved e)
        {
            this.seats = e.Seats.ToList();
        }

        private void OnOrderReservationCompleted(OrderReservationCompleted e)
        {
            this.seats = e.Seats.ToList();
        }

        private void OnOrderExpired(OrderExpired e)
        {
        }

        private void OnOrderConfirmed(OrderConfirmed e)
        {
            this.isConfirmed = true;
        }

        private void OnOrderRegistrantAssigned(OrderRegistrantAssigned e)
        {
        }

        private void OnOrderTotalsCalculated(OrderTotalsCalculated e)
        {
        }
    }
}
