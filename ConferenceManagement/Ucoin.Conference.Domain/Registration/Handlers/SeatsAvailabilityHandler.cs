using Ucoin.Conference.Contracts.Commands.Registration;
using Ucoin.Framework.EventSourcing;
using Ucoin.Framework.Messaging.Handling;
namespace Ucoin.Conference.Domain.Handlers
{
    /// <summary>
    /// Handles commands issued to the seats availability aggregate.
    /// </summary>
    public class SeatsAvailabilityHandler :
        ICommandHandler<MakeSeatReservation>,
        ICommandHandler<CancelSeatReservation>,
        ICommandHandler<CommitSeatReservation>,
        ICommandHandler<AddSeats>,
        ICommandHandler<RemoveSeats>
    {
        private readonly IEventSourcedRepository<SeatsAvailability> repository;

        public SeatsAvailabilityHandler(IEventSourcedRepository<SeatsAvailability> repository)
        {
            this.repository = repository;
        }

        public void Handle(MakeSeatReservation command)
        {
            var availability = this.repository.Get(command.ConferenceId);
            availability.MakeReservation(command.ReservationId, command.Seats);
            this.repository.Save(availability, command.Id.ToString());
        }

        public void Handle(CancelSeatReservation command)
        {
            var availability = this.repository.Get(command.ConferenceId);
            availability.CancelReservation(command.ReservationId);
            this.repository.Save(availability, command.Id.ToString());
        }

        public void Handle(CommitSeatReservation command)
        {
            var availability = this.repository.Get(command.ConferenceId);
            availability.CommitReservation(command.ReservationId);
            this.repository.Save(availability, command.Id.ToString());
        }

        // Commands created from events from the conference BC

        public void Handle(AddSeats command)
        {
            var availability = this.repository.Find(command.ConferenceId);
            if (availability == null)
                availability = new SeatsAvailability(command.ConferenceId);

            availability.AddSeats(command.SeatType, command.Quantity);
            this.repository.Save(availability, command.Id.ToString());
        }

        public void Handle(RemoveSeats command)
        {
            var availability = this.repository.Find(command.ConferenceId);
            if (availability == null)
                availability = new SeatsAvailability(command.ConferenceId);

            availability.RemoveSeats(command.SeatType, command.Quantity);
            this.repository.Save(availability, command.Id.ToString());
        }
    }
}
