using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucoin.Framework.Web
{
    public class BaseController : Controller
    {
        protected IList<IDisposable> disposableObjects;

        public BaseController()
        {
            this.disposableObjects = new List<IDisposable>();
        }

        /// <summary>
        /// 添加需要釋放的對象
        /// </summary>
        /// <param name="obj"></param>
        protected void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (null != disposable)
            {
                this.disposableObjects.Add(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IDisposable obj in this.disposableObjects)
                {
                    try
                    {
                        if (null != obj)
                        {
                            obj.Dispose();
                        }
                    }
                    catch
                    {
                    }
                }
            }

            base.Dispose(disposing);
        }

        public JsonResult AjaxJsonResult(string message, bool isSuccess, object data = null)
        {
            if (isSuccess)
            {
                return AjaxSuccess(message, data);
            }
            else
            {
                return AjaxFailed(message, data);
            }
        }

        public virtual JsonResult AjaxSuccess(string message, object data)
        {
            return Json(new { message = message, data = data, success = true }, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult AjaxFailed(string message, object data)
        {
            return Json(new { message = message, data = data, success = false }, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult AjaxError(Exception ex, object data, bool showStackTrace = true)
        {
            return Json(new { success = false, message = ex.Message, detail = showStackTrace ? ex.StackTrace : string.Empty, data = data }, JsonRequestBehavior.AllowGet);

        }
    }
}
