using System;

namespace Ucoin.Framework.CompareObjects
{
    public class CompareLogic : ICompareLogic
    {
        public ComparisonConfig Config { get; set; }

        public CompareLogic()
        {
            Config = new ComparisonConfig();
        }

        public ComparisonResult Compare(object object1, object object2)
        {
            var result = new ComparisonResult(Config);
            result.Watch.Start();

            var rootComparer = RootComparerFactory.GetRootComparer();
            var parms = new CompareParms
            {
                Config = Config,
                Result = result,
                Object1 = object1,
                Object2 = object2,
                BreadCrumb = string.Empty
            };
            rootComparer.Compare(parms);

            result.Watch.Stop();
            return result;
        }
    }
}
