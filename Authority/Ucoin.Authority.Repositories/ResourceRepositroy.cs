using System.Collections.Generic;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Framework.SqlDb.Repositories;

namespace Ucoin.Authority.Repositories
{
    public class ResourceRepositroy : EfRepository<Resource, int>, IResourceRepositroy
    {
        public ResourceRepositroy(IIdentityRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<Resource> GetResourceListByUserName(string userName)
        {
            var resourceList = new List<Resource>()
            {
                new Resource
                {
                    Id = 1,
                    AppId = 1,
                    Code = "001",
                    Name = "權限管理",
                    ParentId = 0,
                    Sequence = "001",
                    Url = "#",
                    IsVisible = true,
                    IconClass = "icon-sysset",
                },
                new Resource
                {
                    Id = 2,
                    AppId = 1,
                    Code = "001",
                    Name = "菜单导航",
                    ParentId = 1,
                    Sequence = "002",
                    Url = "Rms/Resource",
                    IsVisible = true,
                    IconClass = "icon-chart_organisation",
                }
            };

            return resourceList;
        }


        public IEnumerable<ResourceAction> GetResourceActionsByResourceId(int resourceId)
        {
            var actionList = new List<ResourceAction>
            {
                new ResourceAction
                {
                    Id = 1,
                    ActionId = 1,
                    ResourceId = 2,
                    Url = "Rms/Add",
                    AccessControl = true,
                    Action = new Action
                    {
                        Code = "add",
                        IsPublic = true,
                        Name = "新增",
                        IconClass = "icon-add"
                    }
                },
                new ResourceAction
                {
                    Id = 2,
                    ActionId = 2,
                    ResourceId = 2,
                    Url = "Rms/Edit",
                    AccessControl = true,
                    Action = new Action
                    {
                        Code = "edit",
                        IsPublic = true,
                        Name = "編輯",
                        IconClass = "icon-edit"
                    }
                }
            };

            return actionList;
        }
    }
}
