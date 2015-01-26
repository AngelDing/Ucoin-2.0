
namespace Ucoin.Framework.Entities
{
    public interface IBusinessEntity<TKey> : IEntity<TKey>, IOperateEntity<TKey>, IDeleteEntity
    {
        //public DateTime CreatedDate { get; set; }

        //public int CreatedBy { get; set; }

        //public DateTime UpdatedDate { get; set; }

        //public int UpdatedBy { get; set; }

        //public bool IsDeleted { get; set; }
    }
}
