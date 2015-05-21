using Ucoin.Conference.Contracts.Commands.Registration;
using Ucoin.Conference.Entities.MongoDb;

namespace Ucoin.Conference.Web.Models
{
    public class RegistrationViewModel
    {
        public PricedOrder Order { get; set; }

        public AssignRegistrantDetails RegistrantDetails { get; set; }
    }
}