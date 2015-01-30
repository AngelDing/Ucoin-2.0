using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.CompareObjects
{
    public class EnumeratorComparer : BaseComparer
    {
        private readonly RootComparer rootComparer;
        private CompareParms compareParms;
        private List<string> matchingSpec = new List<string>();

        public EnumeratorComparer(RootComparer comparer, CompareParms parms)
        {
            rootComparer = comparer;
            compareParms = parms;
        }

        public void CompareEnumerator()
        {
            try
            {
                compareParms.Result.AddParent(compareParms.Object1.GetHashCode());
                compareParms.Result.AddParent(compareParms.Object2.GetHashCode());
                //Real Compare
                CompareItems();
            }
            finally
            {
                compareParms.Result.RemoveParent(compareParms.Object1.GetHashCode());
                compareParms.Result.RemoveParent(compareParms.Object2.GetHashCode());
            }            
        }

        private void CompareItems()
        {
            Type type = null;
            GenerateAddAndUpdateDifference(ref type);
            GenerateDeleteDifference(type);

            if (deleteList.Count > 0)
            {
                var newObj = ConvertEnumerableToList(compareParms.Object1) as IList;
                foreach (var obj in deleteList)
                {
                    ReInitDeleteObject(obj);
                    if (compareParms.Object1 is IEnumerable)
                    {                       
                        newObj.Add(obj);                        
                    }
                }
                compareParms.Object1 = newObj;
                GenerateNewParentObject(newObj);
            }            
        }

        private void GenerateNewParentObject(IList newObj)
        {
            var cType = newObj.GetType();
            var pType = compareParms.ParentObject1.GetType();
            var propList = Cache.GetPropertyInfo(pType);
            foreach (var p in propList)
            {
                if (ExcludeLogic.ShouldExcludeMember(compareParms.Config, p))
                {
                    continue;
                }
                var value = p.GetValue(compareParms.ParentObject1);
                if (value == null || TypeHelper.IsCollection(value.GetType()) == false)
                {
                    continue;
                }
                var newValue = ConvertEnumerableToList(value);
                if (newValue.GetType() == cType)
                {
                    p.SetValue(compareParms.ParentObject1, newObj);
                    break;
                }
            }
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

        private void ReInitDeleteObject(object obj)
        {
            var type = obj.GetType();
            var properties = Cache.GetPropertyInfo(type);
            foreach (var pInfo in properties)
            {
                var value = pInfo.GetValue(obj);
                if (value is BaseEntity)
                {
                    pInfo.SetValue(obj, null);
                }
            }
        }

        private void GenerateAddAndUpdateDifference(ref Type type)
        {
            IEnumerator enumerator1 = ((IEnumerable)compareParms.Object1).GetEnumerator();
            IEnumerator enumerator2;
            while (enumerator1.MoveNext())
            {
                if (type == null)
                {
                    type = enumerator1.Current.GetType();
                }
                if (!TypeHelper.IsClass(type))
                {
                    break;
                }
                if (!typeof(IObjectWithState).IsAssignableFrom(type))
                {
                    break;
                }

                matchingSpec = GetMatchingSpec(compareParms.Result, type);

                var currentObject1 = enumerator1.Current;
                var object1 = currentObject1 as IObjectWithState;
                string matchIndex1 = GetMatchIndex(compareParms.Result, matchingSpec, currentObject1);

                var isMatch = false;
                enumerator2 = ((IEnumerable)compareParms.Object2).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    string matchIndex2 = GetMatchIndex(compareParms.Result, matchingSpec, enumerator2.Current);

                    if (matchIndex1 == matchIndex2)
                    {
                        string breadCrumb = string.Format("{0}[{1}]", compareParms.BreadCrumb, matchIndex1);
                        #region Compare Child
                        var childParms = new CompareParms
                        {
                            Result = compareParms.Result,
                            Config = compareParms.Config,
                            ParentObject1 = currentObject1,
                            ParentObject2 = enumerator2.Current,
                            Object1 = currentObject1,
                            Object2 = enumerator2.Current,
                            BreadCrumb = breadCrumb
                        };

                        var areEqual = rootComparer.Compare(childParms);

                        if (areEqual == true)
                        {
                            object1.ObjectState = ObjectStateType.Unchanged;
                        }
                        else
                        {
                            object1.ObjectState = ObjectStateType.Modified;
                            //AddUpdateObject(compareParms.Result, type, object1);
                        }
                        #endregion
                        isMatch = true;
                        break;
                    }
                }
                if (isMatch == false)
                {
                    object1.ObjectState = ObjectStateType.Added;
                    //AddNewObject(compareParms.Result, type, object1);
                }
            }
        }

        private List<object> deleteList = new List<object>();
        private void GenerateDeleteDifference(Type type)
        {
            IEnumerator enumerator2 = ((IEnumerable)compareParms.Object2).GetEnumerator();
            IEnumerator enumerator1;
            while (enumerator2.MoveNext())
            {
                if (type == null)
                {
                    type = enumerator2.Current.GetType();
                }
                if (!TypeHelper.IsClass(type))
                {
                    break;
                }
                if (!typeof(IObjectWithState).IsAssignableFrom(type))
                {
                    break;
                }
                matchingSpec = GetMatchingSpec(compareParms.Result, type);

                var currentObject2 = enumerator2.Current;
                var object2 = currentObject2 as IObjectWithState;
                string matchIndex2 = GetMatchIndex(compareParms.Result, matchingSpec, currentObject2);

                var isMatch = false;
                enumerator1 = ((IEnumerable)compareParms.Object1).GetEnumerator();
                while (enumerator1.MoveNext())
                {
                    string matchIndex1 = GetMatchIndex(compareParms.Result, matchingSpec, enumerator1.Current);
                    if (matchIndex1 == matchIndex2)
                    {
                        isMatch = true;
                        break;
                    }
                }
                if (isMatch == false)
                {
                    object2.ObjectState = ObjectStateType.Deleted;
                    deleteList.Add(object2);                    
                    //AddDeleteObject(compareParms.Result, type, object2);
                }
            }
        }

        private string GetMatchIndex(ComparisonResult result, List<string> spec, object obj)
        {
            var properties = Cache.GetPropertyInfo(obj.GetType()).ToList();
            var sb = new StringBuilder();

            foreach (var item in spec)
            {
                var info = properties.FirstOrDefault(o => o.Name == item);

                var propertyValue = info.GetValue(obj, null);

                if (propertyValue == null)
                {
                    sb.AppendFormat("{0}:(null),", item);
                }
                else
                {
                    sb.AppendFormat("{0}:{1},", item, propertyValue);
                }
            }

            if (sb.Length == 0)
            {
                sb.Append(obj);
            }

            return sb.ToString().TrimEnd(',');
        }

        private List<string> GetMatchingSpec(ComparisonResult result, Type type)
        {
            if (matchingSpec.Any())
            {
                return matchingSpec;
            }

            if (result.Config.CollectionMatchingSpec.Keys.Contains(type))
            {
                return result.Config.CollectionMatchingSpec.First(p => p.Key == type).Value.ToList();
            }

            List<string> list = new List<string>();

            var keyList = Cache.GetKeyPropertyInfo(type);
            if (keyList.Any())
            {
                foreach (var key in keyList)
                {
                    list.Add(key.Name);
                }
            }
            else
            {
                //沒有指定主鍵，則默認為Id
                list.Add("Id");
            }
            return list;
        }
    }
}
