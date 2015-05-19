
namespace Ucoin.Framework.ValueObjects
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PersonalInfo : BaseValueObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+", ErrorMessage = "The provided email address is not valid.")]
        public string Email { get; set; }
    }
}
