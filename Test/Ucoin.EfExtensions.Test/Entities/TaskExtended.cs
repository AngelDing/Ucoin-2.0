using System;
using System.Collections.Generic;
using System.Text;
using Ucoin.Framework.EfExtensions.Audit;

namespace Ucoin.EfExtensions.Test
{
    [Audit]
    public partial class TaskExtended
    {
        public TaskExtended()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public int TaskId { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }

        public virtual Task Task { get; set; }
    }
}