using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 縮略圖信息
    /// </summary>
    [Serializable, DataContract]
    public class ThumbnailEntity : BaseResourceEntity
    {
        /// <summary>
        /// 縮略圖寬度 >1, 必須指定值
        /// </summary>
        [DataMember]
        public int ThumWidth { get; set; }

        /// <summary>
        /// 縮略圖高度 >1
        /// </summary>
        [DataMember]
        public int ThumHeight { get; set; }

        /// <summary>
        /// 是否按指定的寬高來截取圖片，否則按指定的寬或者搞，按寬：高=4:3來縮放
        /// </summary>
        public bool IsInterceptImg
        {
            get
            {
                var isIntercept = false;
                if (ThumHeight > 1 && ThumWidth > 1)
                {
                    isIntercept = true;
                }
                return isIntercept;
            }
        }
    }
}

