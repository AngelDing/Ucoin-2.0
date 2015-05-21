
using System;
namespace Ucoin.Conference.Services
{
    public interface IPaymentService
    {
        object GetThirdPartyProcessorPaymentDetails(Guid paymentId);
    }
}
