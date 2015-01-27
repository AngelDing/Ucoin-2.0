using System;

namespace Ucoin.Framework.ObjectMapper.Test
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
