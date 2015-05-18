
namespace Ucoin.Conference.Contracts.Commands.Payments
{
    using System;
    using Ucoin.Framework.Messaging;

    public class CancelThirdPartyProcessorPayment : ICommand
    {
        public CancelThirdPartyProcessorPayment()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid PaymentId { get; set; }
    }
}
