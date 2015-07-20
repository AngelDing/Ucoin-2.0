using System.Linq;
using System.Collections.Generic;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Authority.IServices;
using Ucoin.Framework.Service;
using Ucoin.Identity.DataObjects;
using System.Threading.Tasks;

namespace Ucoin.Authority.Services
{
    public class ResourceService : BaseService, IResourceService
    {
        private IResourceRepositroy resourceRepo;
        public ResourceService(IIdentityRepositoryContext repoContext, IResourceRepositroy resourceRepo)
            : base(repoContext)
        {
            this.resourceRepo = resourceRepo;
        }

        public Task<IEnumerable<ResourceInfo>> GetResourceListByUserName(string userName)
        {
            var task = Task.Run<IEnumerable<ResourceInfo>>(() =>
            {
                var resourceEntities = resourceRepo.GetResourceListByUserName(userName);
                var infoList = resourceEntities.ToModel<IEnumerable<Resource>, IEnumerable<ResourceInfo>>();
                infoList.LastOrDefault().ParentName = "權限管理";
                return infoList;
            });

            return task;
        }

        public Task<IEnumerable<ActionInfo>> GetResourceActionsByResourceId(int resourceId)
        {
            var task = Task.Run<IEnumerable<ActionInfo>>(() =>
            {
                var resourceEntities = resourceRepo.GetResourceActionsByResourceId(resourceId);
                var infoList = resourceEntities.ToModel<IEnumerable<ResourceAction>, IEnumerable<ActionInfo>>();
                foreach (var info in infoList)
                {
                    var id = info.Id;
                    var entity = resourceEntities.FirstOrDefault(p => p.Id == id);
                    entity.Action.Map(info);
                    info.Id = id;
                }
                return infoList;
            });

            return task;
        }
    }
}