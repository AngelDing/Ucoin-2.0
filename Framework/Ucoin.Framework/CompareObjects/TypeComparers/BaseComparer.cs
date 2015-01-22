using System;
using System.Collections.Generic;
using System.Text;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.CompareObjects
{
    public class BaseComparer
    {
        protected string AddBreadCrumb(Type type, string existing, string name)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(existing))
            {
                existing = type.Name;
            }
            sb.Append(existing);

            bool useName = name.Length > 0;
            if (useName)
            {
                sb.AppendFormat(".");
                sb.Append(name);
            }
            return sb.ToString();
        }

        protected void AddDifference(CompareParms parms)
        {
            Difference difference = new Difference
            {
                PropertyName = parms.BreadCrumb,
                Object1Value = NiceString(parms.Object1),
                Object2Value = NiceString(parms.Object2),
                Object1 = new WeakReference(parms.Object1)
            };

            AddDifference(parms.Result, difference);
        }

        protected void AddDifference(ComparisonResult result, Difference difference)
        {
            difference.ObjectTypeName = difference.Object1 != null && difference.Object1.Target != null
                ? difference.Object1.Target.GetType().Name : "null";

            result.Differences.Add(difference);
        }

        protected void AddNewObject(ComparisonResult result, Type type, IObjectWithState obj)
        {
            if (result.NeedAddList.ContainsKey(type))
            {
                result.NeedAddList[type].Add(obj);
            }
            else
            {
                result.NeedAddList[type] = new List<IObjectWithState> { obj };
            }
        }

        protected void AddUpdateObject(ComparisonResult result, Type type, IObjectWithState obj)
        {
            if (result.NeedUpdateList.ContainsKey(type))
            {
                result.NeedUpdateList[type].Add(obj);
            }
            else
            {
                result.NeedUpdateList[type] = new List<IObjectWithState> { obj };
            }
        }

        protected void AddDeleteObject(ComparisonResult result, Type type, IObjectWithState obj)
        {
            if (result.NeedDeleteList.ContainsKey(type))
            {
                result.NeedDeleteList[type].Add(obj);
            }
            else
            {
                result.NeedDeleteList[type] = new List<IObjectWithState> { obj };
            }
        }

        protected string NiceString(object obj)
        {
            if (obj == null)
            {
                return "(null)";
            }
            try
            {
                return obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
