namespace Ucoin.Conference.Web
{
    using System.Web.Mvc;
    using Ucoin.Conference.Entities.ViewModel;
    using Ucoin.Conference.Services;

    public abstract class ConferenceTenantController : AsyncController
    {
        private ConferenceAlias conferenceAlias;
        private string conferenceCode;

        protected ConferenceTenantController(IConferenceViewService conferenceDao)
        {
            this.ConferenceDao = conferenceDao;
        }

        public IConferenceViewService ConferenceDao { get; private set; }

        public string ConferenceCode
        {
            get
            {
                return this.conferenceCode ??
                    (this.conferenceCode = (string)ControllerContext.RouteData.Values["conferenceCode"]);
            }
            internal set { this.conferenceCode = value; }
        }

        public ConferenceAlias ConferenceAlias
        {
            get
            {
                return this.conferenceAlias ??
                    (this.conferenceAlias = this.ConferenceDao.GetConferenceAlias(this.ConferenceCode));
            }
            internal set { this.conferenceAlias = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!string.IsNullOrEmpty(this.ConferenceCode) &&
                this.ConferenceAlias == null)
            {
                filterContext.Result = new HttpNotFoundResult("Invalid conference code.");
            }
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            if (filterContext.Result is ViewResultBase)
            {
                this.ViewBag.Conference = this.ConferenceAlias;
            }
        }
    }
}