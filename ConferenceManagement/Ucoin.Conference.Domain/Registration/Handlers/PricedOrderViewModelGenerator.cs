
namespace Ucoin.Conference.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Ucoin.Conference.Contracts;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Conference.Entities.MongoDb;
    using Ucoin.Conference.Repositories;
    using Ucoin.Framework.Messaging.Handling;


    public class PricedOrderViewModelGenerator : BaseViewModelGenerator,
        IEventHandler<OrderPlaced>,
        IEventHandler<OrderTotalsCalculated>,
        IEventHandler<OrderConfirmed>,
        IEventHandler<OrderExpired>,
        IEventHandler<SeatAssignmentsCreated>
    {
        private readonly ConferenceMongoRepository<PricedOrder> pricedOrderRepository;
        private readonly ISeatTypeRepository seatTypeRepository;

        public PricedOrderViewModelGenerator(ISeatTypeRepository seatTypeRepository)
        {
            pricedOrderRepository = new ConferenceMongoRepository<PricedOrder>();
            this.seatTypeRepository = seatTypeRepository;
        }

        public void Handle(OrderPlaced @event)
        {
            var dto = new PricedOrder
            {
                OrderId = @event.SourceId,
                ReservationExpirationDate = @event.ReservationAutoExpiration,
                OrderVersion = @event.Version
            };
            pricedOrderRepository.Insert(dto);
        }

        public void Handle(OrderTotalsCalculated @event)
        {
            var seatTypeIds = @event.Lines.OfType<SeatOrderLine>().Select(x => x.SeatType).Distinct().ToArray();

            var dto = pricedOrderRepository.GetBy(x => x.OrderId == @event.SourceId).FirstOrDefault();
            if (!WasNotAlreadyHandled(dto, @event.Version))
            {
                // message already handled, skip.
                return;
            }           

            //var seatTypeDescriptions = GetSeatTypeDescriptions(seatTypeIds, context);
            dto.Lines = new List<PricedOrderLine>();
            for (int i = 0; i < @event.Lines.Length; i++)
            {
                var orderLine = @event.Lines[i];
                var line = new PricedOrderLine
                {
                    LineTotal = orderLine.LineTotal,
                    Position = i,
                };

                var seatOrderLine = orderLine as SeatOrderLine;
                if (seatOrderLine != null)
                {
                    // should we update the view model to avoid losing the SeatTypeId?
                    line.Description = seatTypeRepository.GetByKey(seatOrderLine.SeatType).Name;
                    line.UnitPrice = seatOrderLine.UnitPrice;
                    line.Quantity = seatOrderLine.Quantity;
                }

                dto.Lines.Add(line);
            }

            dto.Total = @event.Total;
            dto.IsFreeOfCharge = @event.IsFreeOfCharge;
            dto.OrderVersion = @event.Version;

            pricedOrderRepository.Update(dto);
        }

        public void Handle(OrderConfirmed @event)
        {
            var dto = pricedOrderRepository.GetBy(p => p.OrderId == @event.SourceId).FirstOrDefault();
            if (WasNotAlreadyHandled(dto, @event.Version))
            {
                dto.ReservationExpirationDate = null;
                dto.OrderVersion = @event.Version;
                pricedOrderRepository.Update(dto);
            }
        }

        public void Handle(OrderExpired @event)
        {
            // No need to keep this priced order alive if it is expired.
            pricedOrderRepository.Delete(p => p.OrderId == @event.SourceId);
        }

        /// <summary>
        /// Saves the seat assignments correlation ID for further lookup.
        /// </summary>
        public void Handle(SeatAssignmentsCreated @event)
        {
            var dto = pricedOrderRepository.GetBy(p => p.OrderId == @event.SourceId).FirstOrDefault();
            dto.AssignmentsId = @event.SourceId;
            // Note: @event.Version does not correspond to order.Version.;
            pricedOrderRepository.Update(dto);
        }

        private static bool WasNotAlreadyHandled(PricedOrder pricedOrder, int eventVersion)
        {
            // This assumes that events will be handled in order, but we might get the same message more than once.
            if (eventVersion > pricedOrder.OrderVersion)
            {
                return true;
            }
            else if (eventVersion == pricedOrder.OrderVersion)
            {
                Trace.TraceWarning(
                    "Ignoring duplicate priced order update message with version {1} for order id {0}",
                    pricedOrder.OrderId,
                    eventVersion);
                return false;
            }
            else
            {
                Trace.TraceWarning(
                    @"Ignoring an older order update message was received with with version {1} for order id {0}, last known version {2}.
This read model generator has an expectation that the EventBus will deliver messages for the same source in order. Nevertheless, this warning can be expected in a migration scenario.",
                    pricedOrder.OrderId,
                    eventVersion,
                    pricedOrder.OrderVersion);
                return false;
            }
        }       
    }
}
