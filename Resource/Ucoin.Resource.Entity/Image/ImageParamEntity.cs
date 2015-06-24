using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 圖片上傳參數
    /// </summary>
    [Serializable, DataContract]
    public class ImageParamEntity : BaseResourceEntity
    {
        /// <summary>
        /// 是否添加水印
        /// </summary>
        [DataMember]
        public bool IsMark { get; set; }

        /// <summary>
        /// 是否產生縮略圖
        /// </summary>
        public bool IsThumbnail
        {
            get
            {
                return ThumbnailInfoList.Count > 0;
            }
        }

        /// <summary>
        /// 縮略圖信息
        /// </summary>
        [DataMember]
        public List<ThumbnailEntity> ThumbnailInfoList { get; set; }

        /// <summary>
        /// 水印圖片地址
        /// </summary>
        [DataMember]
        public string MarkPicFullPath { get; set; }
    }
}
