using System;
using System.Collections.Generic;
using Ucoin.Framework.Entities;
using Ucoin.Framework.ObjectMapper;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.Service
{
    public abstract class BaseService : DisposableObject
    {
        private readonly IRepositoryContext context;

        /// <summary>
        /// 初始化一个<c>ApplicationService</c>类型的实例。
        /// </summary>
        /// <param name="context">用来初始化<c>ApplicationService</c>类型的仓储上下文实例。</param>
        public BaseService(IRepositoryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 获取当前应用层服务所使用的仓储上下文实例。
        /// </summary>
        protected IRepositoryContext DbContext
        {
            get { return this.context; }
        }

        protected override void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// 处理简单的聚合创建逻辑。
        /// </summary>
        /// <typeparam name="TDataObject">数据传输对象类型/View Model</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="models">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="processDto">指定用于处理Data Transferring Object类型的函数。</param>
        /// <param name="processAggregateRoot">指定用于处理聚合根类型的函数。</param>
        /// <returns>包含了已创建的聚合的数据的列表。</returns>
        protected IList<TModel> PerformCreateEntity<TModel, TEntity<TKey>>(IList<TModel> models,
            IRepository<TEntity, TKey> repository,
            Action<TModel> processDto = null,
            Action<TEntity> processAggregateRoot = null)
            where TModel : class, IModel
            where TEntity : class, IAggregateRoot<TKey>
        {
            if (models == null)
            {
                throw new ArgumentNullException("models");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            var result = new List<TModel>();
            if (models.Count > 0)
            {
                var ars = new List<TEntity>();
                foreach (var m in models)
                {
                    if (processDto != null)
                    {
                        processDto(m);
                    }
                    var ar = Mapper.Map<TModel, TEntity>(m);
                    if (processAggregateRoot != null)
                    {
                        processAggregateRoot(ar);
                    }
                    ars.Add(ar);
                    repository.Insert(ar);
                }
                repository.DbContext.Commit();
                ars.ForEach(ar => result.Add(Mapper.Map<TEntity, TModel>(ar)));
            }
            return result;
        }

        /// <summary>
        /// 处理简单的聚合更新操作。
        /// </summary>
        /// <typeparam name="TModel">数据传输对象类型/View Model</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="models">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="idFieldFunc">用于获取数据传输对象唯一标识值的回调函数。</param>
        /// <param name="fieldUpdateAction">用于执行聚合更新的回调函数。</param>
        /// <returns>包含了已更新的聚合的数据的列表。</returns>
        protected IList<TModel> PerformUpdateEntity<TModel, TEntity>(IList<TModel> models,
            IRepository<TEntity, object> repository,
            Func<TModel, string> idFieldFunc,
            Action<TEntity, TModel> fieldUpdateAction)
            where TModel : class, IModel
            where TEntity : class, IAggregateRoot
        {
            if (models == null)
            {
                throw new ArgumentNullException("dataTransferObjects");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (idFieldFunc == null)
            {
                throw new ArgumentNullException("idFieldFunc");
            }
            if (fieldUpdateAction == null)
            {
                throw new ArgumentNullException("fieldAssignmentAction");
            }

            var result = new List<TModel>();
            if (models.Count > 0)
            {
                foreach (var m in models)
                {
                    var id = idFieldFunc(m);
                    var ar = repository.GetByKey(id);
                    fieldUpdateAction(ar, m);
                    repository.Update(ar);
                    result.Add(Mapper.Map<TEntity, TModel>(ar));
                }
                repository.DbContext.Commit();
            }
            return result;
        }

        /// <summary>
        /// 处理简单的删除聚合根的操作。
        /// </summary>
        /// <typeparam name="T">需要删除的聚合根的类型。</typeparam>
        /// <param name="ids">需要删除的聚合根的ID值列表。</param>
        /// <param name="repository">应用于指定聚合根类型的仓储实例。</param>
        /// <param name="preDelete">在指定聚合根被删除前，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        /// <param name="postDelete">在指定聚合根被删除后，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        protected void PerformDeleteEntity<T>(
            IList<object> ids,
            IRepository<T, object> repository,
            Action<object> preDelete = null,
            Action<object> postDelete = null)
            where T : class, IAggregateRoot
        {
            if (ids == null)
                throw new ArgumentNullException("ids");
            if (repository == null)
                throw new ArgumentNullException("repository");
            foreach (var id in ids)
            {
                if (preDelete != null)
                {
                    preDelete(id);
                }
                var ar = repository.GetByKey(id);
                repository.Delete(ar);
                if (postDelete != null)
                {
                    postDelete(id);
                }
            }
            repository.DbContext.Commit();
        }
    }
}
