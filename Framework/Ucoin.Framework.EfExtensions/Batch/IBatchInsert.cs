using System.Collections.Generic;
using System.Data.Entity;

namespace Ucoin.Framework.EfExtensions.Batch
{
    public interface IBatchInsert
    {
        void Insert<TEntity>(DbContext dbContext, IEnumerable<TEntity> entities, int batchSize)
            where TEntity : class;
    }
}
