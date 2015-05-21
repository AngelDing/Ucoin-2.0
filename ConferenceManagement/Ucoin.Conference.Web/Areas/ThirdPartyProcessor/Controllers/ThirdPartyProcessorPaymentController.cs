
namespace Ucoin.Conference.Web.Areas.ThirdPartyProcessor.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Fake 'third party payment processor' web support
    /// </summary>
    public class ThirdPartyProcessorPaymentController : Controller
    {
        private const string returnUrlKey = "returnUrl";
        private const string cancelReturnUrlKey = "cancelReturnUrl";

        [HttpGet]
        public ActionResult Pay(string itemName, decimal itemAmount, string returnUrl, string cancelReturnUrl)
        {
            this.ViewBag.ItemName = itemName;
            this.ViewBag.ItemAmount = itemAmount;
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.CancelReturnUrl = cancelReturnUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Pay(string paymentResult, string returnUrl, string cancelReturnUrl)
        {
            string url;

            if (paymentResult == "accepted")
            {
                url = returnUrl;
            }
            else
            {
                url = cancelReturnUrl;
            }

            return Redirect(url);
        }
    }
}
