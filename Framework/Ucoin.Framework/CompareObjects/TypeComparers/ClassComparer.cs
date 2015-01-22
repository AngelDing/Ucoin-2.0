using System;
using System.Collections;

namespace Ucoin.Framework.CompareObjects
{
    public class ClassComparer : BaseTypeComparer
    {
        private readonly PropertyComparer propertyComparer;

        public ClassComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
            propertyComparer = new PropertyComparer(rootComparer);
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            var isClass = TypeHelper.IsClass(type1) && TypeHelper.IsClass(type2);
            var isInterface = TypeHelper.IsInterface(type1) && TypeHelper.IsInterface(type2);
            return isClass || isInterface;
        }

        public override void CompareType(CompareParms parms)
        {
            //Custom classes that implement IEnumerable may have the same hash code
            //Ignore objects with the same hash code
            if (!(parms.Object1 is IEnumerable) && ReferenceEquals(parms.Object1, parms.Object2))
            {
                return;
            }

            var t1 = parms.Object1.GetType();
            var t2 = parms.Object2.GetType();

            parms.Object1Type = t1;
            parms.Object2Type = t2;

            propertyComparer.PerformCompareProperties(parms);
        }
    }
}
