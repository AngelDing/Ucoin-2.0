
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using Ucoin.Framework.Messaging;

    // Note: ConfirmOrderPayment was renamed to this from V1. Make sure there are no commands pending for processing when this is deployed,
    // otherwise the ConfirmOrderPayment commands will not be processed.
    public class ConfirmOrder : ICommand
    {
        public ConfirmOrder()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
    }
}
