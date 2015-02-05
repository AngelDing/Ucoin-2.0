using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;

namespace Ucoin.Authority.Entities
{
    public class Delegate : EfEntity<int>, IAggregateRoot<int>
    {
        public override IEnumerable<ValidationResult> DoValidate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
