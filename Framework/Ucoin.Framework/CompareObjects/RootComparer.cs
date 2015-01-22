using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ucoin.Framework.CompareObjects
{
    public class RootComparer : BaseComparer
    {
        internal List<BaseTypeComparer> TypeComparers { get; set; }

        public bool Compare(CompareParms parms)
        {
            if (parms.Object1 == null && parms.Object2 == null)
            {
                return true;
            }

            var t1 = parms.Object1 != null ? parms.Object1.GetType() : null;
            var t2 = parms.Object2 != null ? parms.Object2.GetType() : null;

            var typeComparer = TypeComparers.FirstOrDefault(o => o.IsTypeMatch(t1, t2));

            if (typeComparer != null)
            {
                if (IsSameTypes(parms, t1, t2))
                {
                    typeComparer.CompareType(parms);
                }
            }
            else
            {
                if (EitherObjectIsNull(parms))
                {
                    return false;
                }
            }
            return parms.Result.AreEqual;
        }

        private bool IsSameTypes(CompareParms parms, Type t1, Type t2)
        {
            var isSame = true;
            if (parms.Object1 != null && parms.Object2 != null && t1 != t2)
            {
                var difference = new Difference
                {
                    PropertyName = parms.BreadCrumb,
                    Object1Value = t1.FullName,
                    Object2Value = t2.FullName,
                    Object1 = new WeakReference(parms.Object1)
                };

                AddDifference(parms.Result, difference);
                isSame = false;
            }
            return isSame;
        }

        private bool EitherObjectIsNull(CompareParms parms)
        {
            if (parms.Object1 == null || parms.Object2 == null)
            {
                AddDifference(parms);
                return true;
            }
            return false;
        }
    }
}
