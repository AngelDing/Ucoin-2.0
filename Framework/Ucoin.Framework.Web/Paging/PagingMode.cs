
namespace Ucoin.Framework.Web.Paging
{
    public enum PagingMode
    {
        /// <summary>
        /// 只有數字
        /// </summary>
        Numeric,
        /// <summary>
        /// 只有 首頁 上一頁 下一頁 尾頁
        /// </summary>
        NextPreviou,
        /// <summary>
        /// 混合上面兩種模式,另加一個跳轉到第X頁
        /// </summary>
        Hybird
    }
}
