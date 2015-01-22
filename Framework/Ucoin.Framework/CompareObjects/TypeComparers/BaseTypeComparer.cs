using System;

namespace Ucoin.Framework.CompareObjects
{
    public abstract class BaseTypeComparer : BaseComparer
    {
        public RootComparer RootComparer { get; set; }

        protected BaseTypeComparer(RootComparer rootComparer)
        {
            RootComparer = rootComparer;
        }

        public abstract bool IsTypeMatch(Type type1, Type type2);

        public abstract void CompareType(CompareParms parms);
    }
}
