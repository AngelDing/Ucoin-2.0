using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    [DataContract]
    public class PdfRenderParamEntity : BaseResourceEntity
    {
        public PdfRenderParamEntity()
        {
            Margin = new MarginEntity(2, 2, 200, 75);
            PageSizeType = PaperSizeType.A4;
        }
        /// <summary>
        /// 內容URL
        /// </summary>
        [DataMember]
        public string BodyUrl{get;set;}
 
        /// <summary>
        /// 內容HTML（BodyUrl、BodyContent 選其一）
        /// </summary>
        [DataMember]
        public string BodyContent { get; set; }

        /// <summary>
        /// 頁頭URL
        /// </summary>
        [DataMember]
        public string HeaderUrl { get; set; }

        /// <summary>
        /// 頁腳URL
        /// </summary>
        [DataMember]
        public string FooterUrl { get; set; } 

        /// <summary>
        /// 頁面邊距以英寸為單位
        /// </summary>
        [DataMember]
        public MarginEntity Margin { get; set; }

        /// <summary>
        /// 頁面大小
        /// </summary>
        [DataMember]
        public PaperSizeType PageSizeType { get; set; }
        
    }
}
