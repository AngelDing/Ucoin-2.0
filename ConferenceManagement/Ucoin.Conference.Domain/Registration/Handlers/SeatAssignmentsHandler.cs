
namespace Ucoin.Conference.Domain.Handlers
{
    using AutoMapper;
    using Ucoin.Conference.Contracts.Commands.Registration;
    using Ucoin.Conference.Contracts.Events.Registration;
    using Ucoin.Framework.EventSourcing;
    using Ucoin.Framework.Messaging.Handling;

    public class SeatAssignmentsHandler :
        IEventHandler<OrderConfirmed>,
        IEventHandler<OrderPaymentConfirmed>,
        ICommandHandler<UnassignSeat>,
        ICommandHandler<AssignSeat>
    {
        private readonly IEventSourcedRepository<Order> ordersRepo;
        private readonly IEventSourcedRepository<SeatAssignments> assignmentsRepo;

        static SeatAssignmentsHandler()
        {
            Mapper.CreateMap<OrderPaymentConfirmed, OrderConfirmed>();
        }

        public SeatAssignmentsHandler(IEventSourcedRepository<Order> ordersRepo, IEventSourcedRepository<SeatAssignments> assignmentsRepo)
        {
            this.ordersRepo = ordersRepo;
            this.assignmentsRepo = assignmentsRepo;
        }

        public void Handle(OrderPaymentConfirmed @event)
        {
            this.Handle(Mapper.Map<OrderConfirmed>(@event));
        }

        public void Handle(OrderConfirmed @event)
        {
            var order = this.ordersRepo.Get(@event.SourceId);
            var assignments = order.CreateSeatAssignments();
            assignmentsRepo.Save(assignments, null);
        }

        public void Handle(AssignSeat command)
        {
            var assignments = this.assignmentsRepo.Get(command.SeatAssignmentsId);
            assignments.AssignSeat(command.Position, command.Attendee);
            assignmentsRepo.Save(assignments, command.Id.ToString());
        }

        public void Handle(UnassignSeat command)
        {
            var assignments = this.assignmentsRepo.Get(command.SeatAssignmentsId);
            assignments.Unassign(command.Position);
            assignmentsRepo.Save(assignments, command.Id.ToString());
        }
    }
}
