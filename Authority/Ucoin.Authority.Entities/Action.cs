﻿using System;
using System.Collections.Generic;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class Action : EfEntity<int>, IOperateEntity<string>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string IconClass { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public string Sequence { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual ICollection<ResourceAction> ResourceActions { get; set; }
    }
}
