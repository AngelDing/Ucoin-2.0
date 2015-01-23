using System;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Test
{
    public class EFNote : EFEntity<long>
    {
        public string NoteText { get; set; }

        public EFCustomer Customer { get; set; }
    }
}
