using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.ValueObjects;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class UserDelegate : EfEntity<int>, IAggregateRoot<int>, IOperateEntity<string>
    {
        public string Name { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 委託人，存儲數據為：UserId
        /// </summary>
        public int MandatorId { get; set; }

        /// <summary>
        /// 代理人，存儲數據為：UserId
        /// </summary>
        public int MandataryId { get; set; }

        public int PermissionId { get; set; }

        /// <summary>
        /// 生效期限
        /// </summary>
        public DateTimeRange DateRange { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

    }
}
