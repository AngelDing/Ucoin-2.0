
using System.Collections.Generic;

namespace Ucoin.Framework.Paging
{
    public interface IPagingResult<TEntity> : IPaging
    {
        IList<TEntity> Entities { get; set; }
    }
}
