using System;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    [Serializable, DataContract]
    public class BaseReturnEntity
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        [DataMember]
        public bool IsComplete { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [DataMember]
        public string ReturnMessage { get; set; }
    }
}
