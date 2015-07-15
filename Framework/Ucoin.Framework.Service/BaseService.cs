using System;
using System.Collections.Generic;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Models;
using Ucoin.Framework.ObjectMapper;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.Service
{
    public abstract class BaseService : DisposableObject
    {
        private readonly IRepositoryContext context;

        public BaseService(IRepositoryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 获取当前应用层服务所使用的仓储上下文实例。
        /// </summary>
        protected IRepositoryContext Context
        {
            get { return this.context; }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this.Context.Dispose();
            }
        }
    }
}
