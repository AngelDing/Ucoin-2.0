using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Web;
using Ucoin.Framework.Extensions;

namespace Ucoin.Resource.Service
{
    public class CommonHelper
    {
        #region Property
        /// <summary>
        /// 上傳文件保存地絕對路徑
        /// </summary>
        public static string BasePhysicalPathDir
        {
            get
            {
                return ConfigurationManager.AppSettings["ResourceProccess.UploadDir"];
            }
        }

        /// <summary>
        /// 最後文件保存路徑,若未配置文件保存根路徑，则獲取當前應用程序路徑 ps：末尾處始終有 '\'
        /// 1.刪除末尾'\'
        /// 2.在加上'\'
        /// 避免有時有，有時無
        /// </summary>
        public static string PhysicalPathDir
        {
            get
            {
                string physicalPathDir = string.IsNullOrEmpty(BasePhysicalPathDir)
                    ? "d:\\TempUpload\\"
                    : BasePhysicalPathDir;
                return DealSlash(physicalPathDir);
            }
        }

        /// <summary>
        /// 資源服务器地址
        /// </summary>
        public static string GlobalResourceDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["Global.ResourceDomain"];
            }
        }

        public static long MaxResourceLength
        {
            get
            {
                return ConfigurationManager.AppSettings["ResourceProccess.MaxLength"].ToString().ToLong(0);
            }
        }

        /// <summary>
        /// 當前時間 yyyyMMddHHmmssfff
        /// </summary>
        public static string TempDate
        {
            get
            {
                Thread.Sleep(1);
                return DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
        }

        /// <summary>
        /// 當前應用站點物理路徑 ps：這裡始終都有 ''
        /// </summary>
        public static string CurrentAppPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
                //return HttpContext.Current.Request.PhysicalApplicationPath;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 處理斜杠 
        /// </summary>
        public static string DealSlash(string path)
        {
            return string.Format("{0}\\", path.TrimEnd(new char[] { '\\', ' ' }));
        }

        /// <summary>
        /// 文件大小是否超过
        /// </summary>
        /// <param name="totalSize">文件大小</param>
        /// <returns></returns>
        public static bool FileSizeIsOut(long fileLength, long maxLength)
        {
            return fileLength > maxLength ? true : false;
        }

        /// <summary>
        /// 獲得文件虛擬地址
        /// </summary>
        public static string GetVirtualPath(string fullUrl)
        {
            string result = string.Empty;
            string upload = "upload";
            var index = fullUrl.ToLower().IndexOf("upload") + upload.Length;
            //從指定位置獲取,改'/' 為 '\',截取掉起始位置 '\'
            result = fullUrl.Substring(index).Replace('/', '\\').TrimStart(new char[] { '\\', ' ' });
            return result;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsExists(string appPath, string filePath)
        {
            bool flag = true;
            FileInfo fileInfo = new FileInfo(Path.Combine(appPath, filePath));

            if (!fileInfo.Exists)
            {
                flag = false;
            }
            return flag;
        }
        #endregion        
    }
}
