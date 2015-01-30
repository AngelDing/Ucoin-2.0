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
            Childs = new HashSet<ChildNote>();
        }

        public string NoteText { get; set; }

        public int CustomerId { get; set; }

        public virtual ICollection<ChildNote> Childs { get; set; }

        [CompareIgnore]
        public virtual EFCustomer Customer { get; set; }
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
