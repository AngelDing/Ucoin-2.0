using System;

namespace Ucoin.Framework.CompareObjects
{
    public class HashSetComparer : BaseTypeComparer
    {
        public HashSetComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsHashSet(type1) && TypeHelper.IsHashSet(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            var comparer = new EnumeratorComparer(RootComparer, parms);
            comparer.CompareEnumerator();
        }
    }
}
