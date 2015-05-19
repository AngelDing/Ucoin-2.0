namespace Ucoin.Conference.Entities.MongoDb
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using Ucoin.Framework.MongoDb.Entities;

    public class SeatTypeView : StringKeyMongoEntity
    {
        [BsonConstructor]
        public SeatTypeView(Guid id, Guid conferenceId, string name, string description, decimal price, int quantity)
        {
            this.SeatTypeId = id;
            this.ConferenceId = conferenceId;
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
            this.AvailableQuantity = 0;
            this.SeatsAvailabilityVersion = -1;
        }

        protected SeatTypeView()
        {
        }

        public Guid SeatTypeId { get; set; }

        public Guid ConferenceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int AvailableQuantity { get; set; }

        public int SeatsAvailabilityVersion { get; set; }
    }
}
