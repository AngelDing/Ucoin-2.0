using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ucoin.Framework.CompareObjects
{
    public class CompareParms
    {
        public ComparisonConfig Config { get; set; }

        public ComparisonResult Result { get; set; }

        public object Object1 { get; set; }

        public object Object2 { get; set; }

        public string BreadCrumb { get; set; }

        public object ParentObject1 { get; set; }

        public object ParentObject2 { get; set; }

        public Type Object1Type { get; set; }

        public Type Object2Type { get; set; }
    }
}
