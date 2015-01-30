using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Test
{
    [Serializable]
    public class EFCustomer : EFEntity<int>, IAggregateRoot<int>
    {
        public EFCustomer()
        {
            Notes = new HashSet<EFNote>();
        }
        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public EFAddress Address { get; set; }

        public string Email { get; set; }

        public int Sequence { get; set; }

        public virtual ICollection<EFNote> Notes { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                yield return new ValidationResult("UserName must have a value", new[] { "UserName" });
            }
        }
    }
}
