
using System;
using System.Web.Mvc;
using Ucoin.Framework.Web.Paging;
using Ucoin.Framework.Web.Script;

namespace Ucoin.Framework.Web.Extensions
{
    public static class MvcExtensions
    {
        public const string DoPagingPostFile = "Ucoin.Framework.Web.Paging.DoPagingPost.js";

        /// <summary>
        /// 获取分页器上分页按钮的翻页脚本
        /// </summary>
        /// <param name="html"></param>
        /// <param name="isNeedScriptTag">是否要加脚本标签,默认值为true(要加的)</param>
        /// <returns></returns>
        public static MvcHtmlString DoPagingPost(this HtmlHelper html, bool isNeedScriptTag = true)
        {
            IScriptResourceManager mg = new ScriptResourceManager();
            var str = mg.GetScriptResourceContent(DoPagingPostFile);
            if (isNeedScriptTag)
            {
                str = "<script type=\"text/javascript\"> " + str + " </script>";
            }
            return MvcHtmlString.Create(str);
        }

         /// <summary>
        /// 分頁方法
        /// </summary>
        /// <param name="html">擴展自HtmlHelper</param>
        /// <param name="pagingInfo">分頁信息</param>
        /// <param name="pageUrl">獲取分頁按鈕的URL的Func,int參數為當前正在處理的頁</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl,
            Func<int, string> innerHtmlBlock,
            object anchorAttribute = null,
            bool inFormPost = false,
            string elementId = "",
            PagingMode displayMode = PagingMode.Numeric
            )
        {
            return PageHelper.PageLinks(html, pagingInfo, pageUrl, innerHtmlBlock, anchorAttribute, inFormPost, elementId, displayMode);
        }

        /// <summary>
        /// 分頁方法
        /// </summary>
        /// <param name="html">擴展自HtmlHelper</param>
        /// <param name="pagingInfo">分頁信息</param>
        /// <param name="onSelectChange">調整每頁顯示的數量時調用的方法</param>
        /// <param name="pageUrl">獲取分頁按鈕的URL的Func,int參數為當前正在處理的頁</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl,
            Func<int, string> innerHtmlBlock,
            string onSelectChange,
            object anchorAttribute = null,
            bool inFormPost = false,
            string elementId = "",
            PagingMode displayMode = PagingMode.Numeric
            )
        {
            return PageHelper.PageLinks(html, pagingInfo, pageUrl, innerHtmlBlock, onSelectChange, anchorAttribute, inFormPost, elementId, displayMode);
        }
    }
}
