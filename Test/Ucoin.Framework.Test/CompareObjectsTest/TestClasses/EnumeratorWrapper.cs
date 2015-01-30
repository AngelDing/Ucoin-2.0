using System;
using System.Collections.Generic;
using Ucoin.Framework;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Test
{
    public class EnumeratorWrapper : EFEntity<long>
    {
        public EnumeratorWrapper()
        {
            HashSetCollection = new HashSet<HashSetClass>();
            ListCollection = new List<ListSetClass>();
        }

        public int StatusId { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Amount { get; set; }

        public ICollection<HashSetClass> HashSetCollection { get; set; }

        public IList<ListSetClass> ListCollection { get; set; }
    }

    public class ListSetClass : EFEntity<long>
    {
    }

    public class HashSetClass : EFEntity<int>
    {
        public string Name { get; set; }

        public long OrderId { get; set; }

        [CompareIgnore]
        public EnumeratorWrapper HashSetWrapper { get; set; }
    }
}
