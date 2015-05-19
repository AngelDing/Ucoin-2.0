namespace Ucoin.Conference.Entities.MongoDb
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Ucoin.Framework.MongoDb.Entities;

    public class DraftOrder : StringKeyMongoEntity
    {
        public enum States
        {
            PendingReservation = 0,
            PartiallyReserved = 1,
            ReservationCompleted = 2,
            Rejected = 3,
            Confirmed = 4,
        }

        [BsonConstructor]
        public DraftOrder(Guid orderId, Guid conferenceId, States state, int orderVersion = 0)
            : this()
        {
            this.OrderId = orderId;
            this.ConferenceId = conferenceId;
            this.State = state;
            this.OrderVersion = orderVersion;
        }

        protected DraftOrder()
        {
            this.Lines = new ObservableCollection<DraftOrderItem>();
        }

        public Guid OrderId { get; private set; }

        public Guid ConferenceId { get; private set; }

        public DateTime? ReservationExpirationDate { get; set; }

        public ICollection<DraftOrderItem> Lines { get; private set; }

        public int StateValue { get; private set; }

        [BsonIgnore]
        public States State
        {
            get { return (States)this.StateValue; }
            set { this.StateValue = (int)value; }
        }

        public int OrderVersion { get; internal set; }

        public string RegistrantEmail { get; internal set; }

        public string AccessCode { get; internal set; }
    }
}
