using System;

namespace Ucoin.Framework.ObjectMapper.Test
{
    public class OrderItemEntity
    {
        public Guid OrderItemId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
