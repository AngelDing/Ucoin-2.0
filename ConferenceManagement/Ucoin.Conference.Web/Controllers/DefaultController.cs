namespace Ucoin.Conference.Web.Controllers
{
    using System.Web.Mvc;
    using Ucoin.Conference.Services;

    public class DefaultController : Controller
    {
        private readonly IConferenceViewService dao;

        public DefaultController(IConferenceViewService dao)
        {
            this.dao = dao;
        }

        public ActionResult Index()
        {
            return View(this.dao.GetPublishedConferences());
        }
    }
}
