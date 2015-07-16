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
    }
}
