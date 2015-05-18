using System;
using System.Linq;
using System.Data.Entity;
using Ucoin.Framework.Entities;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Ucoin.Framework.Paging;

namespace Ucoin.Framework.SqlDb
{
    public static class EfHelper
    {
        ///// <summary>
        ///// 当EF自动代理类关闭时，在结果集中包含直接被查询对象的指定导航属性的数据, 此方法用於以下include操作：
        ///// IQueryable.IncludeExpand(p => p.A)
        ///// .IncludeExpand(p => p.A.FirstOrDefault().B)
        ///// EF 6自帶的多級加載用法為：
        ///// IQueryable.Include(p => p.A.Select(b => b.B))
        ///// </summary>
        ///// <typeparam name="T">被查询对象的类型</typeparam>
        ///// <param name="query">原始查询集</param>
        ///// <param name="exp">指定导航属性的属性表达式，可以包含First、FirstOrDefault等Lambda方法</param>
        ///// <returns>返回包含指定导航属性数据的查询集</returns>
        //public static IQueryable<T> IncludeExpand<T>(this IQueryable<T> query, Expression<Func<T, dynamic>> exp)
        //    where T : class
        //{
        //    return query.Include(GetStrForInclude(exp));
        //}

        //private static string GetStrForInclude<T>(Expression<Func<T, dynamic>> exp)
        //    where T : class
        //{
        //    string str = string.Empty;
        //    Expression expression = exp.Body;
        //    if (!(expression is MemberExpression))
        //    {
        //        throw new ArgumentException("Must be 'MemberExpression'.");
        //    }
        //    while (expression is MemberExpression || expression is MethodCallExpression)
        //    {
        //        var memberExp = expression as MemberExpression;
        //        if (memberExp != null)
        //        {
        //            str = string.Concat(memberExp.Member.Name, ".", str);
        //            expression = memberExp.Expression;
        //        }
        //        else
        //        {
        //            var callExp = expression as MethodCallExpression;
        //            if (callExp == null || callExp.Arguments == null || callExp.Arguments.Count == 0)
        //            {
        //                throw new ArgumentException("Not a right format expression.");
        //            }
        //            expression = callExp.Arguments[0];
        //        }
        //    }
        //    //if (!string.IsNullOrEmpty(str)) 
        //    //    str = str.Substring(str.Split('.')[0].Length).TrimStart('.').TrimEnd('.');
        //    return str.TrimEnd('.');
        //}

        /// <summary>
        /// 通用的转换实体状态方法
        /// </summary>
        /// <typeparam name="TEntity">要操作的实体</typeparam>
        /// <param name="root">根实体</param>
        public static void ApplyChanges<TEntity>(this DbContext context, TEntity root)
            where TEntity : BaseEntity
        {
            context.Set(root.GetType()).Add(root);
            CheckForEntitiesWithoutStateInterface(context);

            foreach (var entry in context.ChangeTracker.Entries<IObjectWithState>())
            {
                var stateInfo = entry.Entity;
                entry.State = ConvertState(stateInfo.ObjectState);
            }

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                var entity = entry.Entity;
                if (entity.IsPartialUpdate)
                {
                    var type = entity.GetType();
                    context.Set(type).Attach(entity);
                    var puEntry = ((IObjectContextAdapter)context).ObjectContext.
                        ObjectStateManager.GetObjectStateEntry(entity);

                    foreach (var prop in entity.NeedUpdateList.Keys)
                    {
                        var pList = prop.Split('.');
                        //指定屬性更新，如果屬性是值對象，則會更新整個值對象對應的字段
                        //TODO: 是否有方法可以更新指定值對象的指定字段值？
                        puEntry.SetModifiedProperty(pList[0]);
                    }
                }
            }
        }

        private static EntityState ConvertState(ObjectStateType state)
        {
            switch (state)
            {
                case ObjectStateType.Added:
                    return EntityState.Added;
                case ObjectStateType.Modified:
                    return EntityState.Modified;
                case ObjectStateType.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        /// <summary>
        /// 检查实体是否实现了IObjectWithState接口
        /// </summary>
        private static void CheckForEntitiesWithoutStateInterface(DbContext context)
        {
            var entitiesWithoutState = from e in context.ChangeTracker.Entries()
                                       where !(e.Entity is IObjectWithState)
                                       select e;
            if (entitiesWithoutState.Any())
            {
                throw new NotSupportedException("All entities must implement IObjectWithState");
            }
        }
    }
}
