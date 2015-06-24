using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Ucoin.Framework.Result;
using Ucoin.Resource.Entity;
using Ucoin.Framework.Extensions;
using Ucoin.Framework.Utils;

namespace Ucoin.Resource.Service
{
    public class FileUploadManager
    {
        internal BaseResult<List<string>> FileBrowsing(FileBrowsingParamEntity param)
        {
            if (string.IsNullOrWhiteSpace(param.SearchPatterns))
            {
                param.SearchPatterns = "*";
            }
            if (string.IsNullOrWhiteSpace(param.FileDir))
            {
                param.FileDir = string.Empty;
            }
            var result = new BaseResult<List<string>>();
            var rootPath = CommonHelper.PhysicalPathDir;
            var rootURL = string.Concat(CommonHelper.GlobalResourceDomain.TrimEnd('/'), "/Upload/");
            var currentPath = Path.Combine(rootPath, param.FileDir.Trim());
            if (!Directory.Exists(currentPath))
            {
                result.Message  = "指定的搜索目錄不存在。";
                result.Status = ResultStatusType.Faliure;
                return result;
            }           
            var files = new List<string>();
            var patterns = param.SearchPatterns.Trim().Split('|');
            foreach (string pattern in patterns)
            {
                files.AddRange(Directory.GetFiles(currentPath, pattern, param.SearchOption));
            }
            if (files != null && files.Count > 0)
            {
                files.ForEach(p =>
                {
                    result.Value.Add(p.Replace(rootPath, rootURL).FixUrl());
                });
                result.Status = ResultStatusType.Success;
            }
            return result;
        }

        internal FileUploadReturnEntity FileUpload(FileUploadParamEntity param)
        {
            var entity = CheckFileUploadParamEntity(param);
            if (entity.IsComplete == false)
            {
                return entity;
            }

            var fileName = GenerateFileName(param); //文件名称
            var fullPath = GenerateFullPahth(param, fileName);

            try
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    fileStream.Write(param.ResourceBuffer, 0, param.ResourceBuffer.Length);

                    entity.IsComplete = true;
                    var virtualUrl = string.Format("Upload/{0}{1}", param.UploadDir.Replace('\\', '/'), fileName);
                    entity.FileUrl = virtualUrl.FixUrl(CommonHelper.GlobalResourceDomain);
                    entity.FileName = fileName;
                    if (fileStream.Length == param.ResourceTotalSize)
                    {
                        entity.ReturnMessage = "Success";
                    }
                    else
                    {
                        entity.ReturnMessage = "文件上傳成功，但文件有丢失";
                    }
                }
            }
            catch (Exception ex)
            {
                entity.IsComplete = false;
                entity.ReturnMessage = string.Format("程序出错：{0}", ex.Message);
            }
            return entity;
        }

        internal byte[] FileRead(string fileUrl)
        {
            var fileFullPath = GenerateFullPahth(fileUrl);

            FileInfo fi = new FileInfo(fileFullPath);
            if (fi.Exists)
            {
                return fileFullPath.ReadFile();
            }
            
            return null;
        }

        internal FileUploadReturnEntity FileDel(FileUploadParamEntity param)
        {
            var entity = new FileUploadReturnEntity();
            if (string.IsNullOrEmpty(param.ResourceUrl))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "请提供文件路徑";
                return entity;
            }
            var fullPath = GenerateFullPahth(param.ResourceUrl);
            FileInfo fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                    entity.IsComplete = true;
                    entity.ReturnMessage = "Success";
                }
                catch (Exception ex)
                {
                    entity.IsComplete = false;
                    entity.ReturnMessage = string.Format("程序出错：{0}", ex.Message);
                }
            }
            else
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "未找到相关文件";
            }
            return entity;
        }

        #region Private

        private string GenerateFullPahth(string fileUrl)
        {
            var physicalPathDir = CommonHelper.PhysicalPathDir;
            var virtualPath = CommonHelper.GetVirtualPath(fileUrl);
            var fileFullPath = Path.Combine(physicalPathDir, virtualPath);
            return fileFullPath;
        }

        private string GenerateFullPahth(FileUploadParamEntity param, string fileName)
        {
            if (string.IsNullOrEmpty(param.UploadDir))
            {
                var dateNum = DateTime.Now.ToString("yyyyMM");//按月存放
                param.UploadDir = string.Format(@"{0}\", dateNum);
            }
            var dir = CommonHelper.PhysicalPathDir;
            var currentPath = Path.Combine(dir, param.UploadDir); //当前应用程序所在物理路径
            var fullPath = Path.Combine(currentPath, fileName);//文件存储当前应用程序完整物理路径

            //物理路徑是否存在
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
            return fullPath;
        }

        private string GenerateFileName(FileUploadParamEntity param)
        {
            var fileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(param.ResourceName))
            {
                fileName = param.ResourceName;
            }
            else
            {
                fileName = Guid.NewGuid().ToString();
            }

            return fileName + "." + param.Ext;
        }

        private FileUploadReturnEntity CheckFileUploadParamEntity(FileUploadParamEntity param)
        {
            var entity = new FileUploadReturnEntity();
            entity.IsComplete = true;
            if (param.MaxLength <= 0)
            {
                param.MaxLength = CommonHelper.MaxResourceLength;
            }

            if (param.ResourceBuffer == null || param.ResourceBuffer.Length <= 0)
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "请選擇文件";
            }
            else if (string.IsNullOrEmpty(param.Ext))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "请指定文件扩展名";
            }
            else if (CommonHelper.FileSizeIsOut(param.ResourceBuffer.Length.ToString().ToLong(), param.MaxLength))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "文件超过最大限制";
            }
            //else if (param.ResourceTotalSize <= 0)
            //{
            //    entity.IsComplete = false;
            //    entity.ReturnMessage = "请設置所上傳文件的總大小";
            //}

            return entity;
        }
        #endregion
    }
}
