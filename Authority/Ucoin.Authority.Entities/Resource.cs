using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class Resource : EfEntity<int>, IAggregateRoot<int>, IOperateEntity<string>
    {
        public int AppId { get; set; }

        public int ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string IconClass { get; set; }

        public string Description { get; set; }

        public string Sequence { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual ICollection<ResourceButton> ResourceButtons { get; set; }

        public virtual ICollection<ResourceColumn> ResourceColumns { get; set; }

        public virtual Application Application { get; set; }
    }
}
