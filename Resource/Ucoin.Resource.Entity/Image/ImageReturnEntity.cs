using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 圖片上傳返回
    /// </summary>
    [Serializable, DataContract]
    public class ImageReturnEntity : BaseReturnEntity
    {
        /// <summary>
        /// 處理後的圖片信息
        /// </summary>
        public ImageParamEntity ImageInfo { get; set; }
    }
}
