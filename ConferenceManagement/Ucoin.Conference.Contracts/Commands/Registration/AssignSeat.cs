
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;
    using Ucoin.Framework.ValueObjects;

    public class AssignSeat : ICommand
    {
        public AssignSeat()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid SeatAssignmentsId { get; set; }
        public int Position { get; set; }
        public PersonalInfo Attendee { get; set; }
    }
}
