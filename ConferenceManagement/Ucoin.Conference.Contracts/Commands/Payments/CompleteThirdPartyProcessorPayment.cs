
namespace Ucoin.Conference.Contracts.Commands.Payments
{
    using System;
    using Ucoin.Framework.Messaging;

    public class CompleteThirdPartyProcessorPayment : ICommand
    {
        public CompleteThirdPartyProcessorPayment()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid PaymentId { get; set; }
    }
}
