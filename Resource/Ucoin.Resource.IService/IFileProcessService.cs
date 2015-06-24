using System;
using System.Collections.Generic;
using System.ServiceModel;
using Ucoin.ServiceModel.Core;
using Ucoin.Resource.Entity;
using Ucoin.Framework.Result;

namespace Ucoin.Resource.IService
{
    [ServiceContract(Namespace = Constants.Namespace)]
    public interface IFileProcessService
    {
        /// <summary>
        /// 瀏覽目錄
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [OperationContract]
        BaseResult<List<string>> FileBrowsing(FileBrowsingParamEntity param);

        /// <summary>
        /// 文件上传
        /// 必要参数：虚拟目录(FileDir)、文件扩展名(Ext)、Buffer(FileBuffer)、是否添加水印(AddMark),
        /// 若AddMark为True则提供水印图片(MarkPicFullPath)
        /// </summary>
        /// <param name="param">文件上传参数实体</param>
        /// <returns></returns>
        [OperationContract(Name = "FileUpload")]
        FileUploadReturnEntity FileUpload(FileUploadParamEntity param);

        [OperationContract(Name = "FileUploadList")]
        List<FileUploadReturnEntity> FileUpload(List<FileUploadParamEntity> param);

        /// <summary>
        /// 文件删除
        /// 必要参数：请提供文件虚拟Url路径
        /// eg:a/a.jpg or a/a/a.xls
        /// </summary>
        /// <param name="param">文件上传参数实体</param>
        /// <returns></returns>
        [OperationContract]
        FileUploadReturnEntity FileDel(FileUploadParamEntity param);

        /// <summary>
        /// 讀取文件內容
        /// </summary>
        /// <param name="fileVirtualPath"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] FileRead(string fileVirtualPath);
    }
}
