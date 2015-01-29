using System;
using System.Linq;
using System.Data.Entity;
using Ucoin.Framework.Entities;
using System.Data.Entity.Infrastructure;

namespace Ucoin.Framework.EFRepository
{
    public static class EFHelper
    {
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
