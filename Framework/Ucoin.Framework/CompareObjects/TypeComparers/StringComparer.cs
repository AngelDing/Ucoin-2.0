using System;

namespace Ucoin.Framework.CompareObjects
{
    public class StringComparer : BaseTypeComparer
    {
        public StringComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return (TypeHelper.IsString(type1) && TypeHelper.IsString(type2))
                   || (TypeHelper.IsString(type1) && type2 == null)
                   || (TypeHelper.IsString(type2) && type1 == null);
        }

        public override void CompareType(CompareParms parms)
        {
            if (parms.Config.TreatStringEmptyAndNullTheSame
                && ((parms.Object1 == null && parms.Object2 != null && parms.Object2.ToString() == string.Empty)
                    || (parms.Object2 == null && parms.Object1 != null && parms.Object1.ToString() == string.Empty)))
            {
                return;
            }

            if (OneOfTheStringsIsNull(parms))
            {
                return;
            }

            var valOne = parms.Object1 as IComparable;

            if (valOne != null && valOne.CompareTo(parms.Object2) != 0)
            {
                AddDifference(parms);
            }
        }

        private bool OneOfTheStringsIsNull(CompareParms parms)
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
