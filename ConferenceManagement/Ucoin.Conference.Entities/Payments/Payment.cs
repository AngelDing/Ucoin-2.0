using System;
using System.Collections.Generic;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Conference.Entities.Payments
{
    public class Payment : EfEntity<Guid>, IAggregateRoot<Guid>
    {
        public int StateValue { get; set; }

        public Guid PaymentSourceId { get; set; }

        public string Description { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual ICollection<PaymentItem> Items { get; set; }
    }
}
