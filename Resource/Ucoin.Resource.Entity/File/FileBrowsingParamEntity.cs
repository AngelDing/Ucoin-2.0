using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 文檔瀏覽服務參數
    /// </summary>
    [Serializable, DataContract]
    public class FileBrowsingParamEntity
    {
        /// <summary>
        /// 文檔過濾條件，如果此參數不為空，則只查詢符合匹配的文檔，如‘*.csv’
        /// 多個匹配用豎線分隔，如‘*.csv|*.xls’
        /// </summary>
        [DataMember]
        [Description("文檔副檔名過濾條件")]
        public string SearchPatterns { get; set; }

        /// <summary>
        /// 文檔瀏覽起始目錄，相對于設定檔中PhysicalPathDir的相對位置，如“CRMReport\\”
        /// </summary>
        [DataMember]
        [Description("文檔瀏覽起始目錄")]
        public string FileDir { get; set; }

        /// <summary>
        /// 是否搜索子目錄,默認只搜索頂層目錄
        /// </summary>
        [DataMember]
        [Description("是否搜索子目錄")]
        public SearchOption SearchOption { get; set; }
    }
}
