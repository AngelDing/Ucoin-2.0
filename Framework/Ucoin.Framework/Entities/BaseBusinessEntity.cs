using System;

namespace Ucoin.Framework.Entities
{
    /// <summary>
    /// 所有業務單據基類
    /// </summary>
    public class BaseBusinessEntity<TKey> : BaseEntity<TKey>, IOperateEntity<int>, IDeleteEntity
    {
        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
