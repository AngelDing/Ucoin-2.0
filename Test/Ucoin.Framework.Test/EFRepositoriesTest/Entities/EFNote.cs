using System;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Test
{
    [Serializable]
    public class EFNote : EFEntity<long>
    {
        public string NoteText { get; set; }

        public int CustomerId { get; set; }

        public EFCustomer Customer { get; set; }
    }
}
