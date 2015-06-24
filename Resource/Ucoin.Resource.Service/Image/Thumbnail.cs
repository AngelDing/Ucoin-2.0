using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Ucoin.Resource.Entity;

namespace Ucoin.Resource.Service
{
    public class Thumbnail
    {
        private readonly string sourceImagePath;
        private readonly string thumbnailImagePath;
        private readonly ThumbnailEntity thumEntity;

        private static Hashtable HtmlMimeTypes
        {
            get
            {
                var mimes = new Hashtable();
                mimes["jpeg"] = "image/jpeg";
                mimes["jpg"] = "image/jpeg";
                mimes["png"] = "image/png";
                mimes["tif"] = "image/tiff";
                mimes["tiff"] = "image/tiff";
                mimes["bmp"] = "image/bmp";
                mimes["gif"] = "image/gif";

                return mimes;
            }
        }

        private ImageCodecInfo ImageCodecInfo
        {
            get
            {
                return GetCodecInfo((string)HtmlMimeTypes[thumEntity.Ext]);
            }
        }

        /// <summary>
        /// 縮略圖
        /// <param name="param">相關參數</param>
        /// <param name="sPath">原圖路徑</param>
        /// <param name="tPath">縮略圖路徑</param>
        /// </summary>
        public Thumbnail(ThumbnailEntity param, string sPath, string tPath)
        {
            thumEntity = param;
            sourceImagePath = sPath;
            thumbnailImagePath = tPath;
        }

        public void GenerateThumbnailImage()
        {
            if (thumEntity.IsInterceptImg)
            {
                GenerateThumbnailByIntercept();
            }
            else
            {
                //按寬高縮放，若高度  <= 1 則根據寬度計算
                GenerateThumbnailByZoom();
            }
        }

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        private void GenerateThumbnailByIntercept()
        {
            var initImage = Image.FromFile(sourceImagePath);
            var maxWidth = thumEntity.ThumWidth;
            var maxHeight = thumEntity.ThumHeight;

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                SaveImage(initImage, ImageCodecInfo);
                return;
            }

            #region 縮放
            //模版的宽高比例
            double templateRate = (double)maxWidth / maxHeight;
            //原图片的宽高比例
            double initRate = (double)initImage.Width / initImage.Height;
            //原图与模版比例相等，直接缩放
            if (templateRate == initRate)
            {
                //按模版大小生成最终图片
                SaveImage(initImage, maxWidth, maxHeight);
            }
            //原图与模版比例不等，裁剪后缩放
            else
            {
                var pickedImage = GetPickedImage(initImage, templateRate, initRate);
                SaveImage(pickedImage, maxWidth, maxHeight);                
            }
            #endregion
        }

        private Image GetPickedImage(Image initImage, double templateRate, double initRate)
        {
            //裁剪对象
            Image pickedImage = null;
            Graphics pickedG = null;
            //定位
            Rectangle fromR = new Rectangle(0, 0, 0, 0);
            Rectangle toR = new Rectangle(0, 0, 0, 0);

            //宽为标准进行裁剪
            if (templateRate > initRate)
            {
                //裁剪对象实例化
                pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
                pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                //裁剪源定位
                fromR.X = 0;
                fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                fromR.Width = initImage.Width;
                fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

                //裁剪目标定位
                toR.X = 0;
                toR.Y = 0;
                toR.Width = initImage.Width;
                toR.Height = (int)Math.Floor(initImage.Width / templateRate);
            }
            //高为标准进行裁剪
            else
            {
                pickedImage = new System.Drawing.Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
                pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                fromR.Y = 0;
                fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
                fromR.Height = initImage.Height;

                toR.X = 0;
                toR.Y = 0;
                toR.Width = (int)Math.Floor(initImage.Height * templateRate);
                toR.Height = initImage.Height;
            }

            //设置质量
            pickedG.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pickedG.SmoothingMode = SmoothingMode.HighQuality;
            //裁剪
            pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);
            pickedG.Dispose();
            initImage.Dispose();

            return pickedImage;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="sourceImagePath">原图片路径(相对路径)</param>
        /// <param name="thumbnailImagePath">生成的缩略图路径,如果为空则保存为原图片路径(相对路径)</param>
        /// <param name="thumbnailImageWidth">缩略图的宽度（高度与按源图片比例自动生成）</param>
        private void GenerateThumbnailByZoom()
        {
            var newHeight = thumEntity.ThumHeight;
            var newWidth = thumEntity.ThumWidth;
            //从 原图片 创建 Image 对象
            var image = Image.FromFile(sourceImagePath);
            newHeight = ((newWidth / 4) * 3);//高度為寬度的3/4
            int width = image.Width;
            int height = image.Height;

            //计算图片的比例
            if ((((double)width) / ((double)height)) >= 1.3333333333333333f)
            {
                newHeight = ((height * newWidth) / width);
            }
            else
            {
                newWidth = ((width * newHeight) / height);
            }

            SaveImage(image, newWidth, newHeight);
        }

        #region Real Save Image
        public void SaveImage(Image image, int width, int height)
        {
            //用指定的大小和格式初始化 Bitmap 类的新实例
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //从指定的 Image 对象创建新 Graphics 对象
            var graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.High;
            //在指定位置并且按指定大小绘制 原图片 对象
            graphics.DrawImage(image, new Rectangle(0, 0, width, height));
            image.Dispose();
            try
            {
                //将此 原图片 以指定格式并用指定的编解码参数保存到指定文件 
                SaveImage(bitmap, ImageCodecInfo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image">Image 对象</param>
        /// <param name="ici">指定格式的编解码参数</param>
        private void SaveImage(Image image, ImageCodecInfo ici)
        {
            var savePath = thumbnailImagePath;
            //设置 原图片 对象的 EncoderParameters 对象
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)90));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo ici in CodecInfo)
            {

                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }
        #endregion     
    }
}
