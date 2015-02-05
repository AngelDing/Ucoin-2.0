using System;
using System.Collections.Generic;
using Ucoin.Framework.MongoDb.Entities;

namespace Ucoin.MongoRepository.Test
{
    public class Order : StringKeyMongoEntity
    {
        public DateTime PurchaseDate { get; set; }

        public IList<OrderItem> Items;

        public Customer Customer { get; set; }
    }

    public class OrderItem
    {
        public Product Product
        {
            get;
            set;
        }

        public int Quantity { get; set; }
    }
}
