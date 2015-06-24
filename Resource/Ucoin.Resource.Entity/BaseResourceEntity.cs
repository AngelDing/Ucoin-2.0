using System;
using System.IO;
using System.Runtime.Serialization;

namespace Ucoin.Resource.Entity
{
    /// <summary>
    /// 資源基本信息
    /// </summary>
    [Serializable, DataContract]
    public class BaseResourceEntity
    {         
        ///// <summary>
        ///// 資源原始名稱
        ///// </summary>
        //[DataMember]
        //public string ResourceOriginalName { get; set; }

        /// <summary>
        /// 資源名稱
        /// </summary>
        [DataMember]
        public string ResourceName { get; set; }

        /// <summary>
        /// 資源URL
        /// </summary>
        [DataMember]
        public string ResourceUrl { get; set; }

        /// <summary>
        /// 資源完整路徑
        /// </summary>
        [DataMember]
        public string ResourceDir { get; set; }

        /// <summary>
        /// 資源上传目录
        /// eg：Upload\\Image\\
        /// 若为空 则会上传到 Upload\年月   eg：Upload\201203
        /// </summary>
        [DataMember]
        public string UploadDir { get; set; }

        /// <summary> 
        /// 文件扩展类型
        /// eg：doc pdf img ......不需要“.”
        /// </summary>
        [DataMember]
        public string Ext { get; set; }

        /// <summary>
        /// 資源Buffer
        /// </summary>
        [DataMember]
        public byte[] ResourceBuffer{ get; set; }

        /// <summary>
        /// 資源總大小：主要起 校验作用 ，文件是否上传完毕
        /// </summary>
        [DataMember]
        public long ResourceTotalSize { get; set; }

        /// <summary>
        /// 資源限制大小 配置文件读取
        /// 客戶端不配置，會以服務端地配置為準
        /// </summary>
        [DataMember]
        public long MaxLength { get; set; }

        /// <summary>
        /// 上傳物理路徑：当前正在执行的服务器应用程序的根目录的物理文件系统路径
        /// 若未配置，则默認爲當前應用程序的物理路徑
        /// </summary>
        [DataMember]
        public string PhysicalApplicationPath{ get; set; }
    }
}