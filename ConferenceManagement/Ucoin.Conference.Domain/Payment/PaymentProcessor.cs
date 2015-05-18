namespace Ucoin.Payments.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Ucoin.Conference.Contracts.Events.Payments;
    using Ucoin.Conference.Entities.Payments;
    using Ucoin.Framework.Messaging;

    /// <summary>
    /// Represents a payment through a 3rd party system.
    /// </summary>
    public class PaymentProcessor :  IEventPublisher
    {
        private List<IEvent> events = new List<IEvent>();
        public Payment Payment { get; private set; }

        public PaymentProcessor(Guid id, Guid paymentSourceId, 
            string description, decimal totalAmount, IEnumerable<PaymentItem> items)
        {
            Payment = new Payment
            {
                Id = id,
                PaymentSourceId = paymentSourceId,
                Description = description,
                TotalAmount = totalAmount
            };
            Payment.Items.AddRange(items);

            this.AddEvent(new PaymentInitiated { SourceId = id, PaymentSourceId = paymentSourceId });
        }

        public PaymentProcessor(Payment payment)
        {
            this.Payment = payment;
        }

        public PaymentStateType State
        {
            get { return (PaymentStateType)this.Payment.StateValue; }
            internal set { this.Payment.StateValue = (int)value; }
        }

        protected PaymentProcessor()
        {
            this.Payment.Items = new ObservableCollection<PaymentItem>();
        }      

        public IEnumerable<IEvent> Events
        {
            get { return this.events; }
        }

        public void Complete()
        {
            if (this.State != PaymentStateType.Initiated)
            {
                throw new InvalidOperationException();
            }

            this.State = PaymentStateType.Completed;
            var completed = new PaymentCompleted
            { 
                SourceId = this.Payment.Id, 
                PaymentSourceId = this.Payment.PaymentSourceId 
            };
            this.AddEvent(completed);
        }

        public void Cancel()
        {
            if (this.State != PaymentStateType.Initiated)
            {
                throw new InvalidOperationException();
            }

            this.State = PaymentStateType.Rejected;
            var rejected = new PaymentRejected
            { 
                SourceId = this.Payment.Id, 
                PaymentSourceId = this.Payment.PaymentSourceId
            };
            this.AddEvent(rejected);
        }

        protected void AddEvent(IEvent @event)
        {
            this.events.Add(@event);
        }
    }
}
