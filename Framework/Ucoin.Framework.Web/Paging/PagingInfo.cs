
using System;
namespace Ucoin.Framework.Web.Paging
{
    public class PagingInfo
    {
        public bool IsAjaxPost { get; set; }
        public int TotalItems { get; set; }
        private int itemsPerPage = 20;
        public int ItemsPerPage
        {
            get
            {
                return itemsPerPage;
            }
            set
            {
                itemsPerPage = value;
            }
        }
        public int CurrentPage { get; set; }
        private bool showOnMin = true;
        public bool ShowOnMin
        {
            get
            {
                return showOnMin;
            }
            set
            {
                showOnMin = value;
            }
        }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / (ItemsPerPage == 0 ? 1 : ItemsPerPage)); }
        }
        public System.Collections.Generic.List<int> PagingSizes
        {
            get;
            set;
        }
        public string PagingButtonClass
        {
            get;
            set;
        }
        public string PagingNumberClass
        {
            get;
            set;
        }
        public string SelectedPageClass
        {
            get;
            set;
        }
        public string DropDownlistClass
        {
            get;
            set;
        }
        private int hybirdNumberItemCount = 7;

        /// <summary>
        /// 混合模式下數字連接的個數
        /// </summary>
        public int HybirdNumberItemCount
        {
            get
            {
                return hybirdNumberItemCount;
            }
            set
            {
                if (value < 0)
                {
                    hybirdNumberItemCount = 0;
                }
                else
                {
                    hybirdNumberItemCount = value;
                }
            }
        }
        private string nextPageText = "下一頁";
        /// <summary>
        ///  默認為"下一頁"
        /// </summary>
        public string NextPageText
        {
            get
            {
                return nextPageText;
            }
            set
            {
                nextPageText = value;
            }

        }

        private string previouPageText = "上一頁";
        /// <summary>
        /// 默認為"上一頁"
        /// </summary>
        public string PreviouPageText
        {
            get
            {
                return previouPageText;
            }
            set
            {
                previouPageText = value;
            }
        }
        /// <summary>
        /// 默認為"尾頁"
        /// </summary>
        private string lastPageText = "尾頁";
        public string LastPageText
        {
            get
            {
                return lastPageText;
            }
            set
            {
                lastPageText = value;
            }
        }
        private string firstPageText = "首頁";
        /// <summary>
        /// 默認為"首頁"
        /// </summary>
        public string FirstPageText
        {
            get { return firstPageText; }
            set
            {
                firstPageText = value;
            }
        }
        private bool showStatics = true;
        /// <summary>
        /// 是否顯示統計信息(默認顯示)
        /// </summary>
        public bool ShowStatics
        {
            get { return showStatics; }
            set { showStatics = value; }
        }
        private string pageSizeName = "PageSize";
        public string PageSizeName
        {
            get { return pageSizeName; }
            set { pageSizeName = value; }
        }
    }
}
