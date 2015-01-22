using System;
using System.Linq;
using System.Data.Entity;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.EFRepository
{
    public static class EFHelper
    {
        /// <summary>
        /// 通用的转换实体状态方法
        /// </summary>
        /// <typeparam name="TEntity">要操作的实体</typeparam>
        /// <param name="root">根实体</param>
        private static void ApplyChanges<TEntity>(DbContext context, TEntity root) 
            where TEntity : class, IObjectWithState
        {
            using (context)
            {
                context.Set<TEntity>().Add(root);

                CheckForEntitiesWithoutStateInterface(context);   

                foreach (var entry in context.ChangeTracker.Entries<IObjectWithState>())
                {
                    var stateInfo = entry.Entity;
                    entry.State = ConvertState(stateInfo.State);
                }
                context.SaveChanges();
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
