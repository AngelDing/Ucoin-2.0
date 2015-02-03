using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ucoin.Framework.Web.Paging
{
    public static class PageHelper
    {
        /// <summary>
        /// 分頁方法
        /// </summary>
        /// <param name="html">擴展自HtmlHelper</param>
        /// <param name="pagingInfo">分頁信息</param>
        /// <param name="pageUrl">獲取分頁按鈕的URL的Func,int參數為當前正在處理的頁</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl,
            Func<int, string> innerHtmlBlock,
            object anchorAttribute = null,
            bool inFormPost = false,
            string elementId = "",
            PagingMode displayMode = PagingMode.Numeric
            )
        {
            if (pagingInfo.TotalPages <= 1 && !pagingInfo.ShowOnMin)
            {
                return MvcHtmlString.Create("");
            }
            if (inFormPost && string.IsNullOrEmpty(elementId))
            {
                throw new ArgumentException("在设置了inFormPost为True的同时请同时设置elementId");
            }
            StringBuilder result = new StringBuilder();

            Action<int> buildLinkScript = (i) =>
            {
                TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                if (inFormPost)
                {

                    tag.MergeAttribute("page", i.ToString());
                    tag.MergeAttribute("href", "javascript:void(0);");
                    tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                }
                else
                {
                    tag.MergeAttribute("href", pageUrl(i));
                }
                tag.AddCssClass(pagingInfo.PagingNumberClass);
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass(pagingInfo.SelectedPageClass);
                }
                AppendAttribute(tag, anchorAttribute);
                tag.InnerHtml = innerHtmlBlock(i);
                result.Append(tag.ToString());
            };

            Action allnumbericScript = () =>
            {
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    buildLinkScript(i);
                }
            };

            Action firstPart = () =>
            {
                TagBuilder tag = new TagBuilder("a");
                tag.InnerHtml = pagingInfo.FirstPageText;
                tag.MergeAttribute("href", "javascript:void(0);");
                tag.MergeAttribute("page", "1");
                tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                tag.MergeAttribute("class", pagingInfo.PagingButtonClass);
                result.Append(tag.ToString());

                tag.InnerHtml = pagingInfo.PreviouPageText;
                tag.MergeAttribute("page", (pagingInfo.CurrentPage == 1 ? 1 : pagingInfo.CurrentPage - 1).ToString(), true);
                result.Append(tag.ToString());

            };

            Action lastPart = () =>
            {
                TagBuilder tag = new TagBuilder("a");
                tag.InnerHtml = pagingInfo.NextPageText;
                tag.MergeAttribute("href", "javascript:void(0);");
                tag.MergeAttribute("page", (pagingInfo.CurrentPage + 1 <= pagingInfo.TotalPages
                    ? pagingInfo.CurrentPage + 1 :
                      pagingInfo.TotalPages).ToString(), true);
                tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                tag.MergeAttribute("class", pagingInfo.PagingButtonClass);
                result.Append(tag.ToString());
                tag.InnerHtml = pagingInfo.LastPageText;

                tag.MergeAttribute("page", pagingInfo.TotalPages.ToString(), true);
                result.Append(tag.ToString());
            };

            if (displayMode == PagingMode.Numeric)
            {
                allnumbericScript();
            }
            else if (displayMode == PagingMode.NextPreviou)
            {
                firstPart(); lastPart();

            }
            else if (displayMode == PagingMode.Hybird)
            {
                firstPart();
                bool notNeedParting = pagingInfo.HybirdNumberItemCount >= pagingInfo.TotalPages;

                var totalItem = notNeedParting ? pagingInfo.TotalPages : pagingInfo.HybirdNumberItemCount;
                int middleNumber = (int)Math.Ceiling(totalItem / 2d);
                if (notNeedParting)
                {
                    allnumbericScript();
                }
                else
                {
                    var begin = 1;
                    var end = 1;
                    if (pagingInfo.CurrentPage > middleNumber ||
                        pagingInfo.CurrentPage < pagingInfo.TotalPages - middleNumber)
                    {
                        begin = pagingInfo.CurrentPage - middleNumber + (pagingInfo.HybirdNumberItemCount % 2 > 0 ? 1 : 0);
                        end = pagingInfo.CurrentPage + middleNumber - 1;

                    }
                    if (pagingInfo.CurrentPage <= middleNumber)
                    {
                        begin = 1;
                        end = pagingInfo.HybirdNumberItemCount;
                    }
                    if (pagingInfo.TotalPages - pagingInfo.CurrentPage < middleNumber)
                    {
                        begin = pagingInfo.TotalPages - pagingInfo.HybirdNumberItemCount + 1;
                        end = pagingInfo.TotalPages;
                    }
                    while (begin <= end)
                    {
                        buildLinkScript(begin);
                        begin++;
                    }
                }


                lastPart();


            }
            TagBuilder selectWrapper = new TagBuilder("span");
            TagBuilder dropdownlist = new TagBuilder("select");
            dropdownlist.AddCssClass(pagingInfo.DropDownlistClass);
            StringBuilder OptionBuilder = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder option = new TagBuilder("option");
                option.MergeAttribute("value", i.ToString());
                option.SetInnerText(i.ToString());
                if (pagingInfo.CurrentPage == i)
                {
                    option.MergeAttribute("selected", "selected");
                }
                OptionBuilder.Append(option.ToString());
            }
            dropdownlist.InnerHtml = OptionBuilder.ToString();
            dropdownlist.MergeAttribute("onchange", string.Format("doPagingPost(this,'{0}')", elementId));
            selectWrapper.InnerHtml = "第" + dropdownlist.ToString() + "頁";
            result.Append(selectWrapper.ToString());
            if (pagingInfo.PagingSizes == null || !pagingInfo.PagingSizes.Any())
            {
                pagingInfo.PagingSizes = new List<int> { 10,20,30,40,50,60,70,80,90,100};
            }


            List<SelectListItem> list = pagingInfo.PagingSizes.Select(i => new SelectListItem { Text =i.ToString(), Value = i.ToString() }).ToList();
            if (!list.Any(i => i.Value == pagingInfo.ItemsPerPage.ToString()))
            {
                list.Add(new SelectListItem() { Value = pagingInfo.ItemsPerPage.ToString(), Text = pagingInfo.ItemsPerPage.ToString() });
            }
            foreach (var i in list)
            {
                if (i.Value == pagingInfo.ItemsPerPage.ToString())
                {
                    i.Selected = true;
                    break;
                }
            }
            var htmlPageSize = html.DropDownList(pagingInfo.PageSizeName, list, new { onchange = "$(this).closest('form').submit();" });
            if (pagingInfo.IsAjaxPost)
            {
                //bug：採用部份頁，ajax提交查詢時，更改pagesize不能獲得查詢結果，2013-09-13，zjj
                var onchangStr = string.Format("doPagingPostByChangePageSize(this, '{0}')", pagingInfo.PageSizeName);
                htmlPageSize = html.DropDownList(
                    "ddl" + pagingInfo.PageSizeName, list, new { onchange = onchangStr }
                );
            }

            if (pagingInfo.ShowStatics)
            {
                TagBuilder statInfo = new TagBuilder("span");
                statInfo.InnerHtml = string.Format("共{0}頁 每頁{1}條 共{2}條", pagingInfo.TotalPages,htmlPageSize.ToString(), pagingInfo.TotalItems);
                result.Append(statInfo.ToString());
            }


            return MvcHtmlString.Create(result.ToString());
        }
        /// <summary>
        /// 分頁方法
        /// </summary>
        /// <param name="html">擴展自HtmlHelper</param>
        /// <param name="pagingInfo">分頁信息</param>
        /// <param name="onSelectChange">調整每頁顯示的數量時調用的方法</param>
        /// <param name="pageUrl">獲取分頁按鈕的URL的Func,int參數為當前正在處理的頁</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(HtmlHelper html,
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
            if (pagingInfo.TotalPages <= 1 && !pagingInfo.ShowOnMin)
            {
                return MvcHtmlString.Create("");
            }
            if (inFormPost && string.IsNullOrEmpty(elementId))
            {
                throw new ArgumentException("在设置了inFormPost为True的同时请同时设置elementId");
            }
            StringBuilder result = new StringBuilder();

            Action<int> buildLinkScript = (i) =>
            {
                TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                if (inFormPost)
                {
                    tag.MergeAttribute("page", i.ToString());
                    tag.MergeAttribute("href", "javascript:void(0);");
                    tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                }
                else
                {
                    tag.MergeAttribute("href", pageUrl(i));
                }
                tag.AddCssClass(pagingInfo.PagingNumberClass);
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass(pagingInfo.SelectedPageClass);
                }
                AppendAttribute(tag, anchorAttribute);
                tag.InnerHtml = innerHtmlBlock(i);
                result.Append(tag.ToString());
            };

            Action allnumbericScript = () =>
            {
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    buildLinkScript(i);
                }
            };

            Action firstPart = () =>
            {
                TagBuilder tag = new TagBuilder("a");
                tag.InnerHtml = pagingInfo.FirstPageText;
                tag.MergeAttribute("href", "javascript:void(0);");
                tag.MergeAttribute("page", "1");
                tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                tag.MergeAttribute("class", pagingInfo.PagingButtonClass);
                result.Append(tag.ToString());

                tag.InnerHtml = pagingInfo.PreviouPageText;
                tag.MergeAttribute("page", (pagingInfo.CurrentPage == 1 ? 1 : pagingInfo.CurrentPage - 1).ToString(), true);
                result.Append(tag.ToString());

            };

            Action lastPart = () =>
            {
                TagBuilder tag = new TagBuilder("a");
                tag.InnerHtml = pagingInfo.NextPageText;
                tag.MergeAttribute("href", "javascript:void(0);");
                tag.MergeAttribute("page", (pagingInfo.CurrentPage + 1 <= pagingInfo.TotalPages
                    ? pagingInfo.CurrentPage + 1 :
                      pagingInfo.TotalPages).ToString(), true);
                tag.MergeAttribute("onclick", string.Format("doPagingPost(this,'{0}');", elementId));
                tag.MergeAttribute("class", pagingInfo.PagingButtonClass);
                result.Append(tag.ToString());
                tag.InnerHtml = pagingInfo.LastPageText;

                tag.MergeAttribute("page", pagingInfo.TotalPages.ToString(), true);
                result.Append(tag.ToString());
            };

            if (displayMode == PagingMode.Numeric)
            {
                allnumbericScript();
            }
            else if (displayMode == PagingMode.NextPreviou)
            {
                firstPart(); lastPart();

            }
            else if (displayMode == PagingMode.Hybird)
            {
                firstPart();
                bool notNeedParting = pagingInfo.HybirdNumberItemCount >= pagingInfo.TotalPages;

                var totalItem = notNeedParting ? pagingInfo.TotalPages : pagingInfo.HybirdNumberItemCount;
                int middleNumber = (int)Math.Ceiling(totalItem / 2d);
                if (notNeedParting)
                {
                    allnumbericScript();
                }
                else
                {
                    var begin = 1;
                    var end = 1;
                    if (pagingInfo.CurrentPage > middleNumber ||
                        pagingInfo.CurrentPage < pagingInfo.TotalPages - middleNumber)
                    {
                        begin = pagingInfo.CurrentPage - middleNumber + (pagingInfo.HybirdNumberItemCount % 2 > 0 ? 1 : 0);
                        end = pagingInfo.CurrentPage + middleNumber - 1;

                    }
                    if (pagingInfo.CurrentPage <= middleNumber)
                    {
                        begin = 1;
                        end = pagingInfo.HybirdNumberItemCount;
                    }
                    if (pagingInfo.TotalPages - pagingInfo.CurrentPage < middleNumber)
                    {
                        begin = pagingInfo.TotalPages - pagingInfo.HybirdNumberItemCount + 1;
                        end = pagingInfo.TotalPages;
                    }
                    while (begin <= end)
                    {
                        buildLinkScript(begin);
                        begin++;
                    }
                }


                lastPart();


            }
            TagBuilder selectWrapper = new TagBuilder("span");
            TagBuilder dropdownlist = new TagBuilder("select");
            dropdownlist.AddCssClass(pagingInfo.DropDownlistClass);
            StringBuilder OptionBuilder = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder option = new TagBuilder("option");
                option.MergeAttribute("value", i.ToString());
                option.SetInnerText(i.ToString());
                if (pagingInfo.CurrentPage == i)
                {
                    option.MergeAttribute("selected", "selected");
                }
                OptionBuilder.Append(option.ToString());
            }
            dropdownlist.InnerHtml = OptionBuilder.ToString();
            dropdownlist.MergeAttribute("onchange", string.Format("doPagingPost(this,'{0}')", elementId));
            selectWrapper.InnerHtml = "第" + dropdownlist.ToString() + "頁";
            result.Append(selectWrapper.ToString());
            if (pagingInfo.PagingSizes == null || !pagingInfo.PagingSizes.Any())
            {
                pagingInfo.PagingSizes = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            }


            List<SelectListItem> list = pagingInfo.PagingSizes.Select(i => new SelectListItem { Text = i.ToString(), Value = i.ToString() }).ToList();
            if (!list.Any(i => i.Value == pagingInfo.ItemsPerPage.ToString()))
            {
                list.Add(new SelectListItem() { Value = pagingInfo.ItemsPerPage.ToString(), Text = pagingInfo.ItemsPerPage.ToString() });
            }
            foreach (var i in list)
            {
                if (i.Value == pagingInfo.ItemsPerPage.ToString())
                {
                    i.Selected = true;
                    break;
                }
            }
            var htmlPageSize = html.DropDownList(pagingInfo.PageSizeName, list, new { onchange = onSelectChange });
            if (pagingInfo.IsAjaxPost)
            {
                //bug：採用部份頁，ajax提交查詢時，更改pagesize不能獲得查詢結果，2013-09-13，zjj
                var onchangStr = string.Format("doPagingPostByChangePageSize(this, '{0}')", pagingInfo.PageSizeName);
                htmlPageSize = html.DropDownList(
                    "ddl" + pagingInfo.PageSizeName, list, new { onchange = onchangStr }
                );
            }

            if (pagingInfo.ShowStatics)
            {
                TagBuilder statInfo = new TagBuilder("span");
                statInfo.InnerHtml = string.Format("共{0}頁 每頁{1}條 共{2}條", pagingInfo.TotalPages, htmlPageSize.ToString(), pagingInfo.TotalItems);
                result.Append(statInfo.ToString());
            }


            return MvcHtmlString.Create(result.ToString());
        }

        private static void AppendAttribute(TagBuilder tag, object attributeObject)
        {
            if (tag == null || attributeObject == null)
            {
                return;
            }
            var props = attributeObject.GetType().GetProperties(System.Reflection.BindingFlags.GetProperty);
            foreach (var p in props)
            {
                p.GetValue(attributeObject, null);
                tag.MergeAttribute(p.Name, p.GetValue(attributeObject, null).ToString());
            }

        }
    }
}
