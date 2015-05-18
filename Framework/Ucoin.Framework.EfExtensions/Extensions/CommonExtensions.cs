
using System.Collections.Generic;
using System.Linq;
using Ucoin.Framework.Paging;
using Ucoin.Framework.EfExtensions.Future;

namespace Ucoin.Framework.EfExtensions
{
    public static class CommonExtensions
    {
        public static PagingResult<T> Paging<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 20)
            where T : class
        {
            var skipCount = (pageIndex - 1) * pageSize;
            var totalCount = 0;
            IEnumerable<T> entities;

            var objQuery = source.ToObjectQuery();
            if (objQuery == null)
            {
                //當已經獲取到Entities，然後轉為AsQueryable()時，直接分頁即可，不需要再次從數據庫獲取；
                //此種情況多為從服務獲取數據列表后，然後再分頁；
                totalCount = source.Count();
                entities = source.ToList().Skip(skipCount).Take(pageSize);
            }
            else
            {
                var resoultCount = source.FutureCount(objQuery);
                entities = source.Skip(skipCount).Take(pageSize).Future();
                totalCount = resoultCount.Value;
            }
            return new PagingResult<T>(totalCount, pageIndex, pageSize, entities);
        }
    }
}
