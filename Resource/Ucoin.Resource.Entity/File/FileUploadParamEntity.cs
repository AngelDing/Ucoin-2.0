using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 文件上传参数类
    /// </summary>
    [Serializable, DataContract]
    public class FileUploadParamEntity : BaseResourceEntity
    {
        /// <summary>
        /// 文件Guid
        /// 若爲null，则會產生Guid
        /// </summary>
        [DataMember]
        public Guid? FileGuid { get; set; }
    }
}
