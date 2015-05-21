namespace Ucoin.Conference.Web.Controllers
{
    using System;
    using System.Threading;
    using System.Web.Mvc;
    using Ucoin.Conference.Contracts.Commands.Payments;
    using Ucoin.Conference.Services;
    using Ucoin.Framework.Messaging;

    public class PaymentController : Controller
    {
        //private const int WaitTimeoutInSeconds = 5;

        //private readonly ICommandBus commandBus;
        //private readonly IPaymentService paymentDao;

        //public PaymentController(ICommandBus commandBus, IPaymentService paymentDao)
        //{
        //    this.commandBus = commandBus;
        //    this.paymentDao = paymentDao;
        //}

        public ActionResult WaitForPayment()
        {
            return View();
        }

        //public ActionResult ThirdPartyProcessorPayment(string conferenceCode, Guid paymentId, string paymentAcceptedUrl, string paymentRejectedUrl)
        //{
        //    var returnUrl = Url.Action("ThirdPartyProcessorPaymentAccepted", new { conferenceCode, paymentId, paymentAcceptedUrl });
        //    var cancelReturnUrl = Url.Action("ThirdPartyProcessorPaymentRejected", new { conferenceCode, paymentId, paymentRejectedUrl });

        //    // TODO retrieve payment information from payment read model

        //    var paymentDTO = this.WaitUntilAvailable(paymentId);
        //    if (paymentDTO == null)
        //    {
        //        return this.View("WaitForPayment");
        //    }

        //    var paymentProcessorUrl =
        //        this.Url.Action(
        //            "Pay",
        //            "ThirdPartyProcessorPayment",
        //            new
        //            {
        //                area = "ThirdPartyProcessor",
        //                itemName = paymentDTO.Description,
        //                itemAmount = paymentDTO.TotalAmount,
        //                returnUrl,
        //                cancelReturnUrl
        //            });

        //    // redirect to external site
        //    return this.Redirect(paymentProcessorUrl);
        //}

        //public ActionResult ThirdPartyProcessorPaymentAccepted(string conferenceCode, Guid paymentId, string paymentAcceptedUrl)
        //{
        //    this.commandBus.Send(new CompleteThirdPartyProcessorPayment { PaymentId = paymentId });

        //    return this.Redirect(paymentAcceptedUrl);
        //}

        //public ActionResult ThirdPartyProcessorPaymentRejected(string conferenceCode, Guid paymentId, string paymentRejectedUrl)
        //{
        //    this.commandBus.Send(new CancelThirdPartyProcessorPayment { PaymentId = paymentId });

        //    return this.Redirect(paymentRejectedUrl);
        //}

        //private ThirdPartyProcessorPaymentDetails WaitUntilAvailable(Guid paymentId)
        //{
        //    var deadline = DateTime.Now.AddSeconds(WaitTimeoutInSeconds);

        //    while (DateTime.Now < deadline)
        //    {
        //        var paymentDTO = this.paymentDao.GetThirdPartyProcessorPaymentDetails(paymentId);

        //        if (paymentDTO != null)
        //        {
        //            return paymentDTO;
        //        }

        //        Thread.Sleep(500);
        //    }

        //    return null;
        //}
    }
}
