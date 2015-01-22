using System;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.CompareObjects
{
    public class EnumerableComparer : BaseTypeComparer
    {
        private readonly ListComparer listComparer;

        public EnumerableComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
            listComparer = new ListComparer(rootComparer);
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsEnumerable(type1) || TypeHelper.IsEnumerable(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            var t1 = parms.Object1.GetType();
            var t2 = parms.Object2.GetType();

            var l1 = TypeHelper.IsEnumerable(t1) ? ConvertEnumerableToList(parms.Object1) : parms.Object1;
            var l2 = TypeHelper.IsEnumerable(t2) ? ConvertEnumerableToList(parms.Object2) : parms.Object2;

            parms.Object1 = l1;
            parms.Object2 = l2;

            listComparer.CompareType(parms);
        }

        private object ConvertEnumerableToList(object source)
        {
            var type = source.GetType();

            if (type.IsArray)
                return source;

            var genArgs = type.GetGenericArguments();
            var toList = typeof(Enumerable).GetMethod("ToList");
            var constructedToList = toList.MakeGenericMethod(genArgs[0]);
            var resultList = constructedToList.Invoke(null, new[] { source });

            return resultList;
        }
    }
}
