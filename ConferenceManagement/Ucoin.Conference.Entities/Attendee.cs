using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ucoin.Framework.ValueObjects;

namespace Ucoin.Conference.Entities
{
    [ComplexType]
    public class Attendee : BaseValueObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+")]
        public string Email { get; set; }
    }
}
