namespace Ucoin.Conference.Domain
{
    using System.Linq;
    using Ucoin.Conference.Contracts.Commands.Registration;
    using Ucoin.Conference.Domain.Services;
    using Ucoin.Framework.EventSourcing;
    using Ucoin.Framework.Messaging.Handling;

    // Note: ConfirmOrderPayment was renamed to this from V1. Make sure there are no commands pending for processing when this is deployed,
    // otherwise the ConfirmOrderPayment commands will not be processed.
    public class OrderCommandHandler :
        ICommandHandler<RegisterToConference>,
        ICommandHandler<MarkSeatsAsReserved>,
        ICommandHandler<RejectOrder>,
        ICommandHandler<AssignRegistrantDetails>,
        ICommandHandler<ConfirmOrder>
    {
        private readonly IEventSourcedRepository<Order> repository;
        private readonly IPricingService pricingService;

        public OrderCommandHandler(IEventSourcedRepository<Order> repository, IPricingService pricingService)
        {
            this.repository = repository;
            this.pricingService = pricingService;
        }

        public void Handle(RegisterToConference command)
        {
            var items = command.Seats.Select(t => new OrderItem(t.SeatType, t.Quantity)).ToList();
            var order = repository.Find(command.OrderId);
            if (order == null)
            {
                order = new Order(command.OrderId, command.ConferenceId, items, pricingService);
            }
            else
            {
                order.UpdateSeats(items, pricingService);
            }

            repository.Save(order, command.Id.ToString());
        }

        public void Handle(MarkSeatsAsReserved command)
        {
            var order = repository.Get(command.OrderId);
            order.MarkAsReserved(this.pricingService, command.Expiration, command.Seats);
            repository.Save(order, command.Id.ToString());
        }

        public void Handle(RejectOrder command)
        {
            var order = repository.Find(command.OrderId);
            // Explicitly idempotent. 
            if (order != null)
            {
                order.Expire();
                repository.Save(order, command.Id.ToString());
            }
        }

        public void Handle(AssignRegistrantDetails command)
        {
            var order = repository.Get(command.OrderId);
            order.AssignRegistrant(command.FirstName, command.LastName, command.Email);
            repository.Save(order, command.Id.ToString());
        }

        public void Handle(ConfirmOrder command)
        {
            var order = repository.Get(command.OrderId);
            order.Confirm();
            repository.Save(order, command.Id.ToString());
        }
    }
}
