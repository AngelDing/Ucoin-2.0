using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Test
{
    [Serializable]
    public class EFNote : EFEntity<long>
    {
        public EFNote()
        {
            ChildNote = new HashSet<ChildNote>();
        }

        public string NoteText { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<ChildNote> ChildNote { get; set; }

        [CompareIgnore]
        public virtual EFCustomer EFCustomer { get; set; }

        public override IEnumerable<ValidationResult> DoValidate(ValidationContext validationContext)
        {
            return null;
        }
    }

    [Serializable]
    public class ChildNote : EFEntity<long>
    {
        public string Title { get; set; }

        public long NoteId { get; set; }

        [CompareIgnore]
        public virtual EFNote EFNote { get; set; }

        public override IEnumerable<ValidationResult> DoValidate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
