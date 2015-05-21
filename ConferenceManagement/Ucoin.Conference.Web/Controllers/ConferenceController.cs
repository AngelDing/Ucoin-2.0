
namespace Ucoin.Conference.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Ucoin.Conference.Services;

    public class ConferenceController : Controller
    {
        private readonly IConferenceViewService dao;

        public ConferenceController(IConferenceViewService dao)
        {
            this.dao = dao;
        }

        public ActionResult Display(string conferenceCode)
        {
            var conference = this.dao.GetConferenceDetails(conferenceCode);

            // Reply with 404 if not found?
            //if (conference == null)

            return View(conference);
        }
    }
}
