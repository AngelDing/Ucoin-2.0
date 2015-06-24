using System;
using System.Collections.Generic;
using System.ServiceModel;
using Ucoin.Resource.Entity;

namespace Ucoin.Resource.IService
{
    [ServiceContract]
    public interface IImageProcessService
    {
        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <param name="param">圖片上傳参数实体</param>
        /// <returns></returns>
        [OperationContract(Name = "ImgUpload")]
        ImageReturnEntity ImgUpload(ImageParamEntity param);

        /// <summary>
        /// 圖片上傳List
        /// </summary>
        /// <param name="param">圖片上傳参数实体List</param>
        /// <returns></returns>
        [OperationContract(Name = "ImgUploadList")]
        List<ImageReturnEntity> ImgUploadList(List<ImageParamEntity> param);

        /// <summary>
        /// 圖片刪除
        /// </summary>
        /// <param name="param">圖片刪除</param>
        /// <returns></returns>
        [OperationContract(Name = "ImgDel")]
        ImageReturnEntity ImgDel(ImageParamEntity param);
    }
}
