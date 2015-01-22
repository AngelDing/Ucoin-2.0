using System;
using System.Globalization;

namespace Ucoin.Framework.CompareObjects
{
    public class TimespanComparer : BaseTypeComparer
    {
        public TimespanComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsTimespan(type1) && TypeHelper.IsTimespan(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            if (((TimeSpan)parms.Object1).Ticks != ((TimeSpan)parms.Object2).Ticks)
            {
                var difference = new Difference
                {
                    PropertyName = parms.BreadCrumb,
                    Object1Value = ((TimeSpan)parms.Object1).Ticks.ToString(CultureInfo.InvariantCulture),
                    Object2Value = ((TimeSpan)parms.Object2).Ticks.ToString(CultureInfo.InvariantCulture),
                    Object1 = new WeakReference(parms.Object1)
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
