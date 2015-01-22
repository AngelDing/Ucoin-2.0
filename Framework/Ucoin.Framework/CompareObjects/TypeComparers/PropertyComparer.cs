using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ucoin.Framework.CompareObjects
{
    public class PropertyComparer : BaseComparer
    {
        private readonly RootComparer rootComparer;

        public PropertyComparer(RootComparer comparer)
        {
            rootComparer = comparer;
        }

        public void PerformCompareProperties(CompareParms parms)
        {
            var properties = Cache.GetPropertyInfo(parms.Object1Type);

            foreach (PropertyInfo info1 in properties)
            {
                if (info1.CanRead == false)
                {
                    continue;
                }
                if (ExcludeLogic.ShouldExcludeMember(parms.Config, info1))
                {
                    continue;
                }

                var info2 = info1;
                var value1 = info1.GetValue(parms.Object1, null);
                var value2 = info2 != null ? info2.GetValue(parms.Object2, null) : null;

                bool object1IsParent = value1 != null 
                    && (value1 == parms.Object1 || parms.Result.Parents.ContainsKey(value1.GetHashCode()));
                bool object2IsParent = value2 != null 
                    && (value2 == parms.Object2 || parms.Result.Parents.ContainsKey(value2.GetHashCode()));
                //Skip properties where both point to the corresponding parent
                if ((TypeHelper.IsClass(info1.PropertyType) || TypeHelper.IsInterface(info1.PropertyType))
                    && (object1IsParent && object2IsParent))
                {
                    continue;
                }

                var breadCrumb = AddBreadCrumb(parms.Object1Type, parms.BreadCrumb, info1.Name);
                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = value1,
                    Object2 = value2,
                    BreadCrumb = breadCrumb
                };
                rootComparer.Compare(childParms);
            }
        }        
    }
}
