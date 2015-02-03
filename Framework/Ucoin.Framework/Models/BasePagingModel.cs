using System;
using Ucoin.Framework.Paging;

namespace Ucoin.Framework.Models
{
    [Serializable]
    public class BasePagingModel : IModel, IPaging
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
