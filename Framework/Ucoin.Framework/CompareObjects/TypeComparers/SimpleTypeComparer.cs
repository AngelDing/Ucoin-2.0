using System;

namespace Ucoin.Framework.CompareObjects
{
    public class SimpleTypeComparer : BaseTypeComparer
    {
        public SimpleTypeComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsSimpleType(type1) && TypeHelper.IsSimpleType(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            if (parms.Object1 == null || parms.Object2 == null)
            {
                return;
            }

            var valOne = parms.Object1 as IComparable;

            if (valOne == null)
            {
                throw new Exception("Expected value does not implement IComparable");
            }

            if (valOne.CompareTo(parms.Object2) != 0)
            {
                AddDifference(parms);
            }
        }
    }
}
