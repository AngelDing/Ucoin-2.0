
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ucoin.Conference.Contracts.Properties;
    using Ucoin.Framework.Messaging;

    public class AssignRegistrantDetails : ICommand
    {
        public AssignRegistrantDetails()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid OrderId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }
    }
}
