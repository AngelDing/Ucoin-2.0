
namespace Ucoin.Conference.Contracts.Commands.Payments
{
    using System;
    using Ucoin.Framework.Messaging;

    public class InitiateInvoicePayment : ICommand
    {
        public InitiateInvoicePayment()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
