
namespace Ucoin.Conference.Domain
{
    using System.Collections.Generic;
    using Ucoin.Conference.Contracts;

    public struct OrderTotal
    {
        public ICollection<OrderLine> Lines { get; set; }

        public decimal Total { get; set; }
    }
}