using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class Permission : EfEntity<int>, IAggregateRoot<int>, IOperateEntity<string>
    {
        public string Name { get; set; }

        /// <summary>
        /// 权限类型：1.Resource; 2.Button; 3.Column
        /// </summary>
        public byte Type { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

    }
}
