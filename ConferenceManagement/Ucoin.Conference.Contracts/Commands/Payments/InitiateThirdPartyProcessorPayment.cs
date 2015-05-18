
namespace Ucoin.Conference.Contracts.Commands.Payments
{
    using System;
    using System.Collections.Generic;
    using Ucoin.Framework.Messaging;

    public class InitiateThirdPartyProcessorPayment : ICommand
    {
        public class PaymentItem
        {
            public string Description { get; set; }

            public decimal Amount { get; set; }
        }

        public InitiateThirdPartyProcessorPayment()
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<InitiateThirdPartyProcessorPayment.PaymentItem>();
        }

        public Guid Id { get; private set; }

        public Guid PaymentId { get; set; }

        public Guid PaymentSourceId { get; set; }

        public Guid ConferenceId { get; set; }

        public string Description { get; set; }

        public decimal TotalAmount { get; set; }

        public IList<InitiateThirdPartyProcessorPayment.PaymentItem> Items { get; private set; }
    }
}
