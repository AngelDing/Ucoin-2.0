using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;

namespace Ucoin.Authority.Entities
{
    public class Group : EFEntity<int>, IAggregateRoot<int>
    {
        public override IEnumerable<ValidationResult> DoValidate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
