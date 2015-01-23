using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Ucoin.Framework.Entities
{
    [Serializable]
    public class EFEntity<Tkey> : BaseEntity<Tkey>, IObjectWithState, IValidatableObject
    {
        [CompareIgnore]
        public ObjectStateType State { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
