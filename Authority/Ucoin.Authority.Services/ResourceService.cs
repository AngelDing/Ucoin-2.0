using System.Collections.Generic;
using Ucoin.Authority.EFData;
using Ucoin.Authority.Entities;
using Ucoin.Authority.IRepositories;
using Ucoin.Authority.IServices;
using Ucoin.Framework.Service;
using Ucoin.Identity.DataObjects;

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

        public IEnumerable<ResourceInfo> GetResourceListByUserName(string userName)
        {
            var resourceEntities = resourceRepo.GetResourceListByUserName(userName);
            return resourceEntities.ToModel<IEnumerable<Resource>, IEnumerable<ResourceInfo>>();
        }
    }
}