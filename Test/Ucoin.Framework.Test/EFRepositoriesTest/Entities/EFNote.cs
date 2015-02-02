using System;
using System.Collections.Generic;
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
    }

    [Serializable]
    public class ChildNote : EFEntity<long>
    {
        public string Title { get; set; }

        public long NoteId { get; set; }

        [CompareIgnore]
        public virtual EFNote EFNote { get; set; }
    }
}
