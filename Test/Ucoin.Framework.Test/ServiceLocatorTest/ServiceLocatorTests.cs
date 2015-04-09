using Ucoin.Framework.ServiceLocation;

namespace Ucoin.Framework.ServiceLocation.Test
{
    public abstract class ServiceLocatorTests
    {       
        protected readonly IServiceLocator locator;

        protected ServiceLocatorTests()
        {
            locator = CreateServiceLocator();
        }

        protected abstract IServiceLocator CreateServiceLocator();
       
    }
}
