using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Ucoin.Framework.Utils;
using Ucoin.Resource.Entity;
using Ucoin.Resource.IService;
using Ucoin.ServiceModel.Client;

namespace Ucoin.Resource.Test
{
    public partial class UploadResourceTest : Form
    {
        public UploadResourceTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = fileDialog.FileName;
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    var datas = fileName.ReadFile();
                    var param = new FileUploadParamEntity
                    {
                        Ext = Path.GetExtension(fileName).Replace(".", ""),
                        UploadDir = string.Format(@"Test\{0}", DateTime.Now.ToString("yyyyMMdd")),
                        ResourceBuffer = datas,
                        ResourceTotalSize = datas.Length,
                        MaxLength = 5 * 1024 * 1024
                    };

                    var result = ServiceProxy.GetService<IFileProcessService>().FileUpload(param);
                    if(result.IsComplete)
                    {
                        MessageBox.Show("上傳成功！");
                    }
                    else
                    {
                        MessageBox.Show(result.ReturnMessage);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = fileDialog.FileName;
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    var thumbnailList = GetThumbnail(fileName);
                    var datas = fileName.ReadFile();
                    
                    var param = new ImageParamEntity
                    {
                        Ext = Path.GetExtension(fileName).Replace(".", ""),
                        UploadDir = string.Format(@"Image\{0}", DateTime.Now.ToString("yyyyMMdd")),
                        ResourceBuffer = datas,
                        ThumbnailInfoList = thumbnailList,
                        ResourceTotalSize = datas.Length,
                        MaxLength = 5 * 1024 * 1024,
                        //ResourceOriginalName = fileName,
                        IsMark = true,
                        MarkPicFullPath = "wingon_logo2.jpg"
                    };

                    var result = ServiceProxy.GetService<IImageProcessService>().ImgUpload(param);
                    if (result.IsComplete)
                    {
                        MessageBox.Show("上傳成功！");
                    }
                    else
                    {
                        MessageBox.Show(result.ReturnMessage);
                    }
                }
            }
        }

        private List<ThumbnailEntity> GetThumbnail(string fileName)
        {
            var thumbnails = new List<ThumbnailEntity>();
            thumbnails.Add(new ThumbnailEntity
            { //大圖
                ResourceName = string.Format("{0}_{1}", fileName, "big"),
                ThumWidth = 400,//按比例縮放，只需要設置寬度
                ThumHeight = 300
            });
            thumbnails.Add(new ThumbnailEntity
            { //小圖
                ResourceName = string.Format("{0}_{1}", fileName, "sml"),
                ThumWidth = 200,//按比例縮放，只需要設置寬度
                ThumHeight = 150
            });

            return thumbnails;
        }
    }
}
