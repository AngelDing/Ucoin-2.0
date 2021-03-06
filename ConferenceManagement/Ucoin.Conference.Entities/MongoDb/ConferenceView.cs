﻿
namespace Ucoin.Conference.Entities.MongoDb
{
    using System;
    using Ucoin.Framework.MongoDb.Entities;
    using MongoDB.Bson.Serialization.Attributes;

    public class ConferenceView : StringKeyMongoEntity
    {
        [BsonConstructor]
        public ConferenceView(Guid conferenceId, string code, string name, string description,
            string location, string tagline, string twitterSearch, DateTime startDate)
        {
            this.ConferenceId = conferenceId;
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.Location = location;
            this.Tagline = tagline;
            this.TwitterSearch = twitterSearch;
            this.StartDate = startDate;
        }

        public Guid ConferenceId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Tagline { get; set; }

        public string TwitterSearch { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsPublished { get; set; }
    }
}
