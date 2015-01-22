using System;

namespace Ucoin.Framework.Entities
{
    public interface IOperateEntity<T>
    {
        DateTime CreatedDate { get; set; }

        T CreatedBy { get; set; }

        DateTime UpdatedDate { get; set; }

        T UpdatedBy { get; set; }
    }
}
