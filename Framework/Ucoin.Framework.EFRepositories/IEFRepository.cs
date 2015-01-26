
using System.Data.Entity;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.EFRepository
{
    public interface IEFRepository<T, TKey> where T : IAggregateRoot<TKey>
    {
        DbContext DbContext { get; }

        /// <summary>
        /// 更新聚合實體，同時新增，修改或者刪除相關聯有變動的子表信息
        /// </summary>
        /// <param name="entity">聚合實體</param>
        /// <param name="compareResult">新舊對象比較結果</param>
        void FullUpdate(T entity, ComparisonResult compareResult);
    }
}
