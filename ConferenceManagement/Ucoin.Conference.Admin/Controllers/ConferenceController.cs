using AutoMapper;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Web.Mvc;
using Ucoin.Conference.Entities;
using Ucoin.Conference.Services;
using Ucoin.Framework.Utils;

namespace Ucoin.Conference.Admin.Controllers
{
    public class ConferenceController : Controller
    {
        static ConferenceController()
        {
            Mapper.CreateMap<EditableConference, ConferenceInfo>();
        }

        private readonly IConferenceService service;        

        public ConferenceController(IConferenceService service)
        {
            this.service = service;
        }

        //public ConferenceController()
        //{
        //}

        //private ConferenceService Service
        //{
        //    get { return service ?? (service = new ConferenceService(MvcApplication.EventBus)); }
        //}

        public ConferenceInfo Conference { get; private set; }

        /// <summary>
        /// We receive the slug value as a kind of cross-cutting value that 
        /// all methods need and use, so we catch and load the conference here, 
        /// so it's available for all. Each method doesn't need the slug parameter.
        /// </summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var slug = (string)this.ControllerContext.RequestContext.RouteData.Values["slug"];
            if (!string.IsNullOrEmpty(slug))
            {
                this.ViewBag.Slug = slug;
                this.Conference = this.service.FindConference(slug);

                if (this.Conference != null)
                {
                    // check access
                    var accessCode = (string)this.ControllerContext.RequestContext.RouteData.Values["accessCode"];

                    if (accessCode == null || !string.Equals(accessCode, this.Conference.AccessCode, StringComparison.Ordinal))
                    {
                        filterContext.Result = new HttpUnauthorizedResult("Invalid access code.");
                    }
                    else
                    {
                        this.ViewBag.OwnerName = this.Conference.OwnerName;
                        this.ViewBag.WasEverPublished = this.Conference.WasEverPublished;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        #region Conference Details

        public ActionResult Locate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Locate(string email, string accessCode)
        {
            var conference = this.service.FindConference(email, accessCode);
            if (conference == null)
            {
                ModelState.AddModelError(string.Empty, "Could not locate a conference with the provided email and access code.");
                // Preserve input so the user doesn't have to type email again.
                ViewBag.Email = email;

                return View();
            }

            // TODO: This is not very secure. Should use a better authorization infrastructure in a real production system.
            return RedirectToAction("Index", new { slug = conference.Slug, accessCode });
        }

        public ActionResult Index()
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }
            return View(this.Conference);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id,AccessCode,Seats,WasEverPublished")] ConferenceInfo conference)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    conference.Id = GuidHelper.NewSequentialId();
                    this.service.CreateConference(conference);
                }
                catch (DuplicateNameException e)
                {
                    ModelState.AddModelError("Slug", e.Message);
                    return View(conference);
                }

                return RedirectToAction("Index", new { slug = conference.Slug, accessCode = conference.AccessCode });
            }

            return View(conference);
        }

        public ActionResult Edit()
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }
            return View(this.Conference);
        }

        [HttpPost]
        public ActionResult Edit(EditableConference conference)
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var edited = Mapper.Map(conference, this.Conference);
                this.service.UpdateConference(edited);
                return RedirectToAction("Index", new { slug = edited.Slug, accessCode = edited.AccessCode });
            }

            return View(this.Conference);
        }

        [HttpPost]
        public ActionResult Publish()
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            this.service.Publish(this.Conference.Id);

            return RedirectToAction("Index", new { slug = this.Conference.Slug, accessCode = this.Conference.AccessCode });
        }

        [HttpPost]
        public ActionResult Unpublish()
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            this.service.Unpublish(this.Conference.Id);

            return RedirectToAction("Index", new { slug = this.Conference.Slug, accessCode = this.Conference.AccessCode });
        }

        #endregion

        #region Seat Types

        public ViewResult Seats()
        {
            return View();
        }

        public ActionResult SeatGrid()
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            return PartialView(this.service.FindSeatTypes(this.Conference.Id));
        }

        public ActionResult SeatRow(Guid id)
        {
            return PartialView("SeatGrid", new SeatType[] { this.service.FindSeatType(id) });
        }

        public ActionResult CreateSeat()
        {
            return PartialView("EditSeat");
        }

        [HttpPost]
        public ActionResult CreateSeat(SeatType seat)
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                seat.Id = GuidHelper.NewSequentialId();
                this.service.CreateSeat(this.Conference.Id, seat);

                return PartialView("SeatGrid", new SeatType[] { seat });
            }

            return PartialView("EditSeat", seat);
        }

        public ActionResult EditSeat(Guid id)
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            return PartialView(this.service.FindSeatType(id));
        }

        [HttpPost]
        public ActionResult EditSeat(SeatType seat)
        {
            if (this.Conference == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.service.UpdateSeat(this.Conference.Id, seat);
                }
                catch (ObjectNotFoundException)
                {
                    return HttpNotFound();
                }

                return PartialView("SeatGrid", new SeatType[] { seat });
            }

            return PartialView(seat);
        }

        [HttpPost]
        public void DeleteSeat(Guid id)
        {
            this.service.DeleteSeat(id);
        }

        #endregion

        #region Orders

        public ViewResult Orders()
        {
            var orders = this.service.FindOrders(this.Conference.Id);

            return View(orders);
        }

        #endregion
    }
}