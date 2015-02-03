using System;
using System.IO;
using System.Text;

namespace Ucoin.Framework.Web.Script
{
    public class ScriptResourceManager : IScriptResourceManager
    {
        public ScriptResourceManager()
        {
        }

        /// <summary>
        /// 获取资源的内容，即JS文件的内容
        /// </summary>
        /// <param name="fileName">脚本文件的全名，即："GroupTour.Web.Resource" + JS文件名</param>
        /// <returns></returns>
        public virtual string GetScriptResourceContent(string fileName)
        {
            var sb = new StringBuilder();
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(fileName))
            {
                try
                {
                    using (var sr = new StreamReader(stream))
                    {
                        try
                        {
                            return sr.ReadToEnd();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            if (null != sr)
                            {
                                sr.Close();
                                sr.Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (null != stream)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }


        string IScriptResourceManager.GetScriptResourceContent(string fileName)
        {
            return this.GetScriptResourceContent(fileName);
        }
    }
}
