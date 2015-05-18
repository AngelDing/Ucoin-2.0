namespace Ucoin.Conference.Contracts.Events.Payments
{
    using System;
    using Ucoin.Framework.Messaging;

    public class PaymentRejected : IEvent
    {
        public Guid SourceId { get; set; }

        public Guid PaymentSourceId { get; set; }
    }
}
