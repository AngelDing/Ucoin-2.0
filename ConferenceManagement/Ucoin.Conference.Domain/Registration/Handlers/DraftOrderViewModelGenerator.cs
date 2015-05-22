
namespace Ucoin.Conference.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AutoMapper;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Framework.Messaging.Handling;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Repositories;
    using Ucoin.Conference.Contracts;

    public class DraftOrderViewModelGenerator : BaseViewModelGenerator,
        IEventHandler<OrderPlaced>, IEventHandler<OrderUpdated>,
        IEventHandler<OrderPartiallyReserved>, IEventHandler<OrderReservationCompleted>,
        IEventHandler<OrderRegistrantAssigned>,
        IEventHandler<OrderConfirmed>, IEventHandler<OrderPaymentConfirmed>
    {
        private readonly ConferenceMongoRepository<DraftOrder> draftOrderRepository;

        static DraftOrderViewModelGenerator()
        {
            // Mapping old version of the OrderPaymentConfirmed event to the new version.
            // Currently it is being done explicitly by the consumer, but this one in particular could be done
            // at the deserialization level, as it is just a rename, not a functionality change.
            Mapper.CreateMap<OrderPaymentConfirmed, OrderConfirmed>();
        }

        public DraftOrderViewModelGenerator()
        {
            draftOrderRepository = new ConferenceMongoRepository<DraftOrder>();
        }

        public void Handle(OrderPlaced @event)
        {
            var dto = new DraftOrder(@event.SourceId, @event.ConferenceId, (int)DraftOrder.States.PendingReservation, @event.Version)
            {
                AccessCode = @event.AccessCode,
            };
            dto.Lines.AddRange(@event.Seats.Select(seat => new DraftOrderItem(seat.SeatType, seat.Quantity)));

            draftOrderRepository.Insert(dto);
        }

        public void Handle(OrderRegistrantAssigned @event)
        {
            var dto = GetDraftOrder(@event.SourceId);
            if (WasNotAlreadyHandled(dto, @event.Version))
            {
                dto.RegistrantEmail = @event.Email;
                dto.OrderVersion = @event.Version;

                draftOrderRepository.Update(dto);
            }
        }

        private DraftOrder GetDraftOrder(Guid orderId)
        {
            return draftOrderRepository.GetBy(p => p.OrderId == orderId).FirstOrDefault();
        }

        public void Handle(OrderUpdated @event)
        {
            var dto = GetDraftOrder(@event.SourceId);
            if (WasNotAlreadyHandled(dto, @event.Version))
            {
                var newItems = @event.Seats.Select(seat => new DraftOrderItem(seat.SeatType, seat.Quantity));
                dto.Lines = newItems.ToList();
                dto.State = DraftOrder.States.PendingReservation;
                dto.OrderVersion = @event.Version;

                draftOrderRepository.Update(dto);
            }
        }

        public void Handle(OrderPartiallyReserved @event)
        {
            this.UpdateReserved(@event.SourceId, @event.ReservationExpiration, DraftOrder.States.PartiallyReserved, @event.Version, @event.Seats);
        }

        public void Handle(OrderReservationCompleted @event)
        {
            this.UpdateReserved(@event.SourceId, @event.ReservationExpiration, DraftOrder.States.ReservationCompleted, @event.Version, @event.Seats);
        }

        public void Handle(OrderPaymentConfirmed @event)
        {
            this.Handle(Mapper.Map<OrderConfirmed>(@event));
        }

        public void Handle(OrderConfirmed @event)
        {
            var dto = GetDraftOrder(@event.SourceId);
            if (WasNotAlreadyHandled(dto, @event.Version))
            {
                dto.State = DraftOrder.States.Confirmed;
                dto.OrderVersion = @event.Version;
                draftOrderRepository.Update(dto);
            }
        }

        private void UpdateReserved(Guid orderId, DateTime reservationExpiration, DraftOrder.States state, int orderVersion, IEnumerable<SeatQuantity> seats)
        {
            var dto = GetDraftOrder(orderId);
            if (WasNotAlreadyHandled(dto, orderVersion))
            {
                foreach (var seat in seats)
                {
                    var item = dto.Lines.Single(x => x.SeatType == seat.SeatType);
                    item.ReservedSeats = seat.Quantity;
                }

                dto.State = state;
                dto.ReservationExpirationDate = reservationExpiration;

                dto.OrderVersion = orderVersion;

                draftOrderRepository.Update(dto);
            }
        }

        private static bool WasNotAlreadyHandled(DraftOrder draftOrder, int eventVersion)
        {
            // This assumes that events will be handled in order, but we might get the same message more than once.
            if (eventVersion > draftOrder.OrderVersion)
            {
                return true;
            }
            else if (eventVersion == draftOrder.OrderVersion)
            {
                Trace.TraceWarning(
                    "Ignoring duplicate draft order update message with version {1} for order id {0}",
                    draftOrder.OrderId,
                    eventVersion);
                return false;
            }
            else
            {
                Trace.TraceWarning(
                    @"An older order update message was received with with version {1} for order id {0}, last known version {2}.
This read model generator has an expectation that the EventBus will deliver messages for the same source in order.",
                    draftOrder.OrderId,
                    eventVersion,
                    draftOrder.OrderVersion);
                return false;
            }
        }
    }
}
