using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class Application : EfEntity<int>, IAggregateRoot<int>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
    }
}
