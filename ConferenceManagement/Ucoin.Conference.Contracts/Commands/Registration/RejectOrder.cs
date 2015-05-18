namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;

    public class RejectOrder : ICommand
    {
        public RejectOrder()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
    }
}
