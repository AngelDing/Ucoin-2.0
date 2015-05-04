using System;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;
using Ucoin.Framework.Utils;

namespace Ucoin.Conference.Entities
{
    public class SeatType : EfEntity<Guid>, IAggregateRoot<Guid>
    {
        public SeatType()
        {
            this.Id = GuidHelper.NewSequentialId();
        }

        [Required]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Range(0, 100000)]
        public int Quantity { get; set; }

        [Range(0, 50000)]
        public decimal Price { get; set; }
    }
}