
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;

    public class UnassignSeat : ICommand
    {
        public UnassignSeat()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid SeatAssignmentsId { get; set; }
        public int Position { get; set; }
    }
}
