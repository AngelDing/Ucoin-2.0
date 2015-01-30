using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucoin.Framework.CompareObjects
{
    public static class RootComparerFactory
    {
        private static readonly object locker = new object();
        private static RootComparer rootComparer;

        public static RootComparer GetRootComparer()
        {
            lock (locker)
            {
                if (rootComparer == null)
                {
                    rootComparer = BuildRootComparer();
                }
            }
            return rootComparer;
        }

        private static RootComparer BuildRootComparer()
        {
            rootComparer = new RootComparer();

            rootComparer.TypeComparers = new List<BaseTypeComparer>();
            rootComparer.TypeComparers.Add(new HashSetComparer(rootComparer));
            rootComparer.TypeComparers.Add(new ListComparer(rootComparer));
            rootComparer.TypeComparers.Add(new EnumComparer(rootComparer));
            rootComparer.TypeComparers.Add(new StringComparer(rootComparer));
            rootComparer.TypeComparers.Add(new SimpleTypeComparer(rootComparer));
            rootComparer.TypeComparers.Add(new TimespanComparer(rootComparer));
            rootComparer.TypeComparers.Add(new ClassComparer(rootComparer)); //類比對應該放在最後

            return rootComparer;
        }
    }
}
