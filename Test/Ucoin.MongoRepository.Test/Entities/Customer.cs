using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Ucoin.Framework.MongoDb.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ucoin.MongoRepository.Test
{
    [CollectionName("CustomersTest")]
    public class Customer : StringKeyMongoEntity
    {
        /// <summary>
        /// 可手動顯示賦值為自增長
        /// </summary>
        public int CustId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Address HomeAddress { get; set; }

        public IList<Order> Orders { get; set; }

        public DateTime CreateDate { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                yield return new ValidationResult("FirstName must have a value", new[] { "FirstName" });
            }
        }
    }

    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        [BsonIgnoreIfNull]
        public string Country { get; set; }
    }
}
