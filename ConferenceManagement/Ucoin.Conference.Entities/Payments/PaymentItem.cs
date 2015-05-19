using System;
using Ucoin.Framework.SqlDb.Entities;
using Ucoin.Framework.Utils;

namespace Ucoin.Conference.Entities
{
    public class PaymentItem : EfEntity<Guid>
    {
        public PaymentItem(string description, decimal amount)
        {
            this.Id = GuidHelper.NewSequentialId();
            this.Description = description;
            this.Amount = amount;
        }

        public string Description { get; private set; }

        public decimal Amount { get; private set; }
    }
}
