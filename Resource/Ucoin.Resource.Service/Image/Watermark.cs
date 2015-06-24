using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Ucoin.Resource.Service
{
    public class Watermark
    {
        private readonly string oldPath;
        private readonly string newPath;
        private readonly WatermarkType watermarkType;
        private readonly string sWaterMarkContent;
        private Image image = null;

        public WatermarkPositionType? WatermarkPosition { get; set; }

        /// <summary>
        /// 添加水印(分图片水印与文字水印两种)
        /// </summary>
        /// <param name="oPath">原图片绝对地址</param>
        /// <param name="nPath">新图片放置的绝对地址</param>
        /// <param name="wType">要添加的水印的类型</param>
        /// <param name="content">水印内容，若添加文字水印，此即为要添加的文字；
        /// 若要添加图片水印，此为图片的路径</param>
        public Watermark(string oPath, string nPath, WatermarkType wType, string content)
        {
            oldPath = oPath;
            newPath = nPath;
            watermarkType = wType;
            sWaterMarkContent = content;            
        }
        
        public void GenerateWatermark()
        {
            try
            {
                image = Image.FromFile(oldPath);
                using (var b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb))
                {
                    using (var g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.High;
                        g.DrawImage(image, 0, 0, image.Width, image.Height);

                        switch (watermarkType)
                        {
                            case WatermarkType.ImageMark:
                                this.GenerateImageWatermark(g);
                                break;
                            case WatermarkType.TextMark:
                                this.GenerateTextWatermark(g);
                                break;
                            default:
                                break;
                        }
                    }
                    b.Save(newPath);
                }
            }
            catch
            {
                if (File.Exists(oldPath))
                {
                    image.Dispose();
                    File.Delete(oldPath);
                }
            }

            finally
            {
                if (File.Exists(oldPath))
                {
                    image.Dispose();
                    File.Delete(oldPath);
                }
            }
        }

        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        private void GenerateTextWatermark(Graphics picture)
        {
            var _width = image.Width;
            var _height = image.Height;
            // 确定水印文字的字体大小
            int[] sizes = new int[] { 32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();

            for (int i = 0; i < sizes.Length; i++)
            {
                crFont = new Font("Arial Black", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(sWaterMarkContent, crFont);

                if ((ushort)crSize.Width < (ushort)_width)
                {
                    break;
                }
            }

            // 生成水印图片（将文字写到图片中）
            Bitmap floatBmp = new Bitmap((int)crSize.Width + 3,
                (int)crSize.Height + 3, PixelFormat.Format32bppArgb);
            Graphics fg = Graphics.FromImage(floatBmp);
            PointF pt = new PointF(0, 0);

            // 画阴影文字
            Brush TransparentBrush0 = new SolidBrush(Color.FromArgb(255, Color.Black));
            Brush TransparentBrush1 = new SolidBrush(Color.FromArgb(255, Color.Black));
            fg.DrawString(sWaterMarkContent, crFont, TransparentBrush0, pt.X, pt.Y + 1);
            fg.DrawString(sWaterMarkContent, crFont, TransparentBrush0, pt.X + 1, pt.Y);

            fg.DrawString(sWaterMarkContent, crFont, TransparentBrush1, pt.X + 1, pt.Y + 1);
            fg.DrawString(sWaterMarkContent, crFont, TransparentBrush1, pt.X, pt.Y + 2);
            fg.DrawString(sWaterMarkContent, crFont, TransparentBrush1, pt.X + 2, pt.Y);

            TransparentBrush0.Dispose();
            TransparentBrush1.Dispose();

            // 画文字
            fg.SmoothingMode = SmoothingMode.HighQuality;
            fg.DrawString(sWaterMarkContent,
                crFont, new SolidBrush(Color.White),
                pt.X, pt.Y, StringFormat.GenericDefault);

            // 保存刚才的操作
            fg.Save();
            fg.Dispose();

            // 将水印图片加到原图中
            this.GenerateImageWatermark(picture, new Bitmap(floatBmp));
        }

        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="iTheImage">Image对象（以此图片为水印）</param>
        private void GenerateImageWatermark(Graphics picture, Image iTheImage)
        {
            //Image watermark = new Bitmap(iTheImage);
            var watermark = iTheImage;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
				new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
				new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
				new float[] {0.0f, 0.0f, 0.0f, 0.3f, 0.0f},
				new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
			};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            var rectangle = GenerateRectangle(watermark);

            picture.DrawImage(
                watermark,
                rectangle,
                0,
                0,
                watermark.Width,
                watermark.Height,
                GraphicsUnit.Pixel,
                imageAttributes
            );
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        private Rectangle GenerateRectangle(Image watermark)
        {
            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;

            var _width = image.Height;
            var _height = image.Height;

            double bl = GetImageRatio(watermark);
            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);

            if (!WatermarkPosition.HasValue)
            {
                WatermarkPosition = WatermarkPositionType.WMP_Right_Bottom;
            }
            switch (WatermarkPosition)
            {
                case WatermarkPositionType.WMP_Left_Top:
                    xpos = 10;
                    ypos = 10;
                    break;
                case WatermarkPositionType.WMP_Right_Top:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case WatermarkPositionType.WMP_Right_Bottom:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case WatermarkPositionType.WMP_Left_Bottom:
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }

            return new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight);
        }

        private double GetImageRatio(Image watermark)
        {
            double bl = 1d;
            var _width = image.Height;
            var _height = image.Height;

            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

            }
            else if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((_width * watermark.Height) > (_height * watermark.Width))
                {
                    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);
                }
                else
                {
                    bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
                }
            }
            return bl;
        }

        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        private void GenerateImageWatermark(Graphics picture)
        {
            var picPath = sWaterMarkContent;
            using (var img = new Bitmap(picPath))
            {
                this.GenerateImageWatermark(picture, img);
            }
        }
    }
}
