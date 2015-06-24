using System;
using System.Collections.Generic;
using Ucoin.Framework.Result;
using Ucoin.Resource.Entity;
using Ucoin.Resource.IService;

namespace Ucoin.Resource.Service
{
    public class FileProcessService : IFileProcessService
    {
        private Lazy<FileUploadManager> manager = new Lazy<FileUploadManager>(() =>
        {
            return new FileUploadManager();
        }, true);


        public BaseResult<List<string>> FileBrowsing(FileBrowsingParamEntity param)
        {
            return manager.Value.FileBrowsing(param);
        }

        public FileUploadReturnEntity FileUpload(FileUploadParamEntity param)
        {
            return manager.Value.FileUpload(param);
        }

        public List<FileUploadReturnEntity> FileUpload(List<FileUploadParamEntity> param)
        {
            var returnInfos = new List<FileUploadReturnEntity>();
            param.ForEach(i =>
            {
                var temp = manager.Value.FileUpload(i);
                returnInfos.Add(temp);
            });
            return returnInfos;
        }

        public FileUploadReturnEntity FileDel(FileUploadParamEntity param)
        {
            return manager.Value.FileDel(param);
        }

        public byte[] FileRead(string fileVirtualPath)
        {
            return manager.Value.FileRead(fileVirtualPath);
        }
    }
}
