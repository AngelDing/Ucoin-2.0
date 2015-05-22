using MongoDB.Bson.Serialization.Attributes;
using System;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.Conference.Entities.MongoDb
{
    public class PaymentView : StringKeyMongoEntity
    {
        [BsonConstructor]
        public PaymentView(Guid id, PaymentStateType state, Guid paymentSourceId, 
            string description, decimal totalAmount)
        {
            this.Id = id;
            this.State = state;
            this.PaymentSourceId = paymentSourceId;
            this.Description = description;
            this.TotalAmount = totalAmount;
        }

        protected PaymentView()
        {
        }

        public Guid Id { get; private set; }

        public int StateValue { get; private set; }

        [BsonIgnore]
        public PaymentStateType State
        {
            get { return (PaymentStateType)this.StateValue; }
            set { this.StateValue = (int)value; }
        }

        public Guid PaymentSourceId { get; private set; }

        public string Description { get; private set; }

        public decimal TotalAmount { get; private set; }
    }
}