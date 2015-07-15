using Microsoft.Practices.Unity;
using System.Web.Http.Dependencies;

namespace Ucoin.Framework.Web.Resolver
{
    public class UnityApiResolver : BaseResolver, IDependencyResolver
    {
        public UnityApiResolver(IUnityContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityApiResolver(child);
        }
    }
}
