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

        /// <summary>
        /// 將變更後的對象同之前的對象進行比較，得出不同之處
        /// </summary>
        /// <param name="newObj">新對象</param>
        /// <param name="preObj">之前舊對象</param>
        /// <returns></returns>
        public ComparisonResult Compare(object newObj, object preObj)
        {
            var result = new ComparisonResult(Config);
            result.Watch.Start();

            var rootComparer = RootComparerFactory.GetRootComparer();
            var parms = new CompareParms
            {
                Config = Config,
                Result = result,
                Object1 = newObj,
                Object2 = preObj,
                BreadCrumb = string.Empty
            };
            rootComparer.Compare(parms);

            result.Watch.Stop();
            return result;
        }
    }
}
