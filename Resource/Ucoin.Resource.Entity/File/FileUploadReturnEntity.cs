using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 文件上传返回类
    /// </summary>
    [Serializable, DataContract]
    public class FileUploadReturnEntity : BaseReturnEntity
    {
        /// <summary>
        /// 文件Url
        /// </summary>
        [DataMember]
        public string FileUrl { get; set; }

        /// <summary>
        /// 文件名稱
        /// </summary>
        [DataMember]
        public string FileName{ get; set; }
    }
}
