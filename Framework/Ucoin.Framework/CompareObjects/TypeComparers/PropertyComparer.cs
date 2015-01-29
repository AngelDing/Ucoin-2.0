using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Ucoin.Framework.Entities;
using Ucoin.Framework.ValueObjects;
using System.ComponentModel;

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
                var differentCount = parms.Result.Differences.Count();
                rootComparer.Compare(childParms);
                var newDifferentCount = parms.Result.Differences.Count();
                if (differentCount != newDifferentCount)
                {
                    GenerateUpdatePropertyList(parms, info1, value1, value2);
                }
            }
        }

        private void GenerateUpdatePropertyList(CompareParms parms, PropertyInfo pInfo, object value1, object value2)
        {
            var childType = GetChildType(value1, value2);
            //var parentType = GetParentType(parms);           

            if ((value1 is BaseValueObject) || (value2 is BaseValueObject))
            {
                GenerateUpdatePropertyList(parms, pInfo);
            }
            else if (TypeHelper.IsSimpleType(childType) || TypeHelper.IsString(childType)
                || TypeHelper.IsTimespan(childType) || TypeHelper.IsEnum(childType))
            {
                GenerateUpdatePropertyList(parms, pInfo);
            }
        }

        private void GenerateUpdatePropertyList(CompareParms parms, PropertyInfo pInfo)
        {
            if ((parms.Object1 is BaseEntity) || (parms.Object1 is BaseEntity))
            {
                var baseEntity = parms.Object1 as BaseEntity;
                baseEntity.IsPartialUpdate = true;
                baseEntity.ObjectState = ObjectStateType.Modified;
                baseEntity.NeedUpdateList.Add(pInfo.Name, null);
            }

            //if (parms.Result.UpdatePropertyList.ContainsKey(parentType) == false)
            //{
            //    parms.Result.UpdatePropertyList.Add(parentType, new List<UpdatedPropertyInfo>());
            //}

            //var entityList = parms.Result.UpdatePropertyList[parentType];
            //var keyId = GetParentEntityId(parms);
            //var entity = entityList.FirstOrDefault(p => p.Id.ToString() == keyId.ToString());
            //if (entity == null)
            //{
            //    var update = new UpdatedPropertyInfo
            //    {
            //        Id = keyId,
            //        PropertyList = new List<string>
            //        {
            //            info1.Name
            //        }
            //    };
            //    entityList.Add(update);
            //}
            //else
            //{
            //    if (entity.PropertyList.Contains(info1.Name) == false)
            //    {
            //        entity.PropertyList.Add(info1.Name);
            //    }
            //}
        }

        //private object GetParentEntityId(CompareParms parms)
        //{
        //    object id = null;
        //    var type = GetParentType(parms);
        //    var pdList = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().ToList();
        //    var pd = pdList.Where(i => i.DisplayName == "Id").FirstOrDefault();
        //    if (pd != null)
        //    {
        //        id = pd.GetValue(parms.Object1);
        //    }
        //    return id;
        //}

        //private Type GetParentType(CompareParms parms)
        //{
        //    if (parms.Object1Type != null)
        //    {
        //        return parms.Object1Type;
        //    }
        //    else
        //    {
        //        return parms.Object2Type;
        //    }
        //}

        private Type GetChildType(object value1, object value2)
        {
            Type type = null;
            if (value1 != null)
            {
                type = value1.GetType();
            }
            else if (value2 != null)
            {
                type = value2.GetType();
            }
            return type;
        }        
    }
}
