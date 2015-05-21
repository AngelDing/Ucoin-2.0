
using System.Web.Mvc;

namespace Ucoin.Conference.Web.Areas.ThirdPartyProcessor
{
    public class ThirdPartyProcessorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ThirdPartyProcessor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Pay",
                "payment",
                new { controller = "ThirdPartyProcessorPayment", action = "Pay" });
        }
    }
}
