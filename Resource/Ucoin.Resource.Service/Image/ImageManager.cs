using System;
using System.IO;
using Ucoin.Resource.Entity;
using Ucoin.Framework.Extensions;

namespace Ucoin.Resource.Service
{
    public class ImageManager
    {
        private string basicImageDir;
        private string basicImageFullPath;
        private string basicImageName;
        private ImageParamEntity imageParam = null;

        internal ImageReturnEntity ImageUpload(ImageParamEntity param)
        {
            imageParam = param;
            var returnEntity = CheckImageParamEntity();
            if (returnEntity.IsComplete == false)
            {
                return returnEntity;
            }
            try
            {
                SaveOriginalImage();
                if (imageParam.IsMark)
                {
                    GenerateWatermark();
                }
                if (imageParam.IsThumbnail)
                {
                    foreach (var t in imageParam.ThumbnailInfoList)
                    {
                        t.Ext = param.Ext;
                        GenerateThumbnail(t);
                    }
                }
                returnEntity.IsComplete = true;
                returnEntity.ImageInfo = imageParam;
            }
            catch (Exception ex)
            {
                returnEntity.IsComplete = false;
                returnEntity.ReturnMessage = string.Format("程序出错：{0}", ex.Message);
            }

            return returnEntity;
        }

        private void GenerateThumbnail(ThumbnailEntity thumEntity)
        {
            string thumFileName = GetImageName(thumEntity.Ext);
            //TODO:如果指定縮略圖的存放地址，按指定的位置存放
            string thumFileFullPath = GetFileFullPath(basicImageDir, thumFileName); //保存路徑，與原圖一樣
            var thum = new Thumbnail(thumEntity, basicImageFullPath, thumFileFullPath);
            thum.GenerateThumbnailImage();

            thumEntity.ResourceName = thumFileName;
            var tempDir = Path.Combine(imageParam.UploadDir, thumFileName);
            string globalImageDomain = CommonHelper.GlobalResourceDomain;//圖片服務器地址
            thumEntity.ResourceUrl = string.Format("/Upload/{0}", tempDir).FixUrl(globalImageDomain);
        }

        private void GenerateWatermark()
        {
            var newFileName = GetImageName(imageParam.Ext);
            var newFileFullPath = GetFileFullPath(basicImageDir, newFileName);

            string watermarkPic = Path.Combine(CommonHelper.CurrentAppPath, imageParam.MarkPicFullPath);
            var watermark = new Watermark(basicImageFullPath, newFileFullPath,
                WatermarkType.ImageMark, watermarkPic);
            watermark.WatermarkPosition = WatermarkPositionType.WMP_Right_Bottom;
            watermark.GenerateWatermark();
            //圖片添加水印后，後續操作都是針對有水印的圖片
            basicImageFullPath = newFileFullPath;
            basicImageName = newFileName;
        }

        private void SaveOriginalImage()
        {
            basicImageDir = Path.Combine(CommonHelper.PhysicalPathDir, imageParam.UploadDir);
            basicImageName = GetImageName(imageParam.Ext);
            basicImageFullPath = GetFileFullPath(basicImageDir, basicImageName);

            if (!Directory.Exists(basicImageDir))
            {
                Directory.CreateDirectory(basicImageDir);
            }
            using (var fileStream = new FileStream(basicImageFullPath, FileMode.Create, FileAccess.ReadWrite))
            {
                fileStream.Write(imageParam.ResourceBuffer, 0, imageParam.ResourceBuffer.Length);
            }
        }

        private string GetImageName(string ext)
        {
            var imgName = CommonHelper.TempDate; //文件名稱
            imgName = string.Format("{0}.{1}", imgName, ext); //文件名稱 + 擴展名
            return imgName;
        }

        private string GetFileFullPath(string fullPath, string fileName)
        {
            var fileFullPath = Path.Combine(fullPath, fileName);//根路徑 + 虛擬路徑 + 文件名
            return fileFullPath;
        }

        private ImageReturnEntity CheckImageParamEntity()
        {
            var entity = new ImageReturnEntity();
            entity.IsComplete = true;
            //文件大小
            imageParam.MaxLength = imageParam.MaxLength <= 0 
                ? CommonHelper.MaxResourceLength 
                : imageParam.MaxLength;

            if (imageParam.ResourceBuffer == null || imageParam.ResourceBuffer.Length <= 0)
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "請選擇圖片";
            }
            else if (string.IsNullOrEmpty(imageParam.Ext))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "请指定圖片擴展名 eg：jpg";
            }
            else if (CommonHelper.FileSizeIsOut(imageParam.ResourceBuffer.Length, imageParam.MaxLength))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = string.Format("文件超過最大限制 系统配置：{0}，实际上传{1}", 
                    imageParam.MaxLength, imageParam.ResourceBuffer.Length);
            }
            else if (imageParam.IsMark && string.IsNullOrEmpty(imageParam.MarkPicFullPath))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "請提供水印圖片地址";
            }
            else if (imageParam.IsMark && !string.IsNullOrEmpty(imageParam.MarkPicFullPath) 
                && !CommonHelper.IsExists(CommonHelper.CurrentAppPath, imageParam.MarkPicFullPath))
            {
                entity.IsComplete = false;
                entity.ReturnMessage = "提供的水印圖片不存在";
            }
            return entity;
        }
    }
}
