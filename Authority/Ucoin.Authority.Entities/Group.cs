using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    /// <summary>
    /// 數據權限
    /// </summary>
    public class Group : EfEntity<int>, IAggregateRoot<int>, IOperateEntity<string>
    {
        public string Name { get; set; }

        /// <summary>
        /// 組類型：如：用戶組，項目組，部門，區域等
        /// </summary>
        public byte Type { get; set; }

        public int RefId { get; set; }

        public int ParentRefId { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
