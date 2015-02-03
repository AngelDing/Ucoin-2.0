using System;
using System.Collections;
using System.Globalization;

namespace Ucoin.Framework.CompareObjects
{
    public class ListComparer : BaseTypeComparer
    {
        public ListComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(System.Type type1, System.Type type2)
        {
            //TODO: 如果type2是HashSet，則需將type2轉成IList來比較
            return TypeHelper.IsIList(type1) && TypeHelper.IsIList(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            var comparer = new EnumeratorComparer(RootComparer, parms);
            comparer.CompareEnumerator();
        }
    }
}
