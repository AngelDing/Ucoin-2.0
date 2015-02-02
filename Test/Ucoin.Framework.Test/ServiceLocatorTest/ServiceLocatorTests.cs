using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections;
using System.Configuration;
using Ucoin.Framework;
using Ucoin.Framework.Service;
using Xunit;

namespace Ucoin.Framework.ServiceLocator.Test
{
    public class ServiceLocatorTests
    {
        [Fact]
        public void service_locator_get_instance_test()
        {
            ILogger instance = Ucoin.Framework.Service.ServiceLocator.GetService<ILogger>();
            Assert.NotNull(instance);
        }

        [Fact]
        public void service_locator_activation_exception_test()
        {
            Assert.Throws<ActivationException>(delegate
            {
                Ucoin.Framework.Service.ServiceLocator.GetService<IDictionary>();
            });
        }

        [Fact]
        public void service_locator_get_instance_by_manual_register_test()
        {
            var container = Ucoin.Framework.Service.ServiceLocator.GetUnityContainer();
            if (container.IsRegistered<IUnregisteredLogger>() == false)
            {
                container.RegisterType<IUnregisteredLogger, AdvancedLogger>();
            }

            var instance = Ucoin.Framework.Service.ServiceLocator.GetService<IUnregisteredLogger>();
            Assert.NotNull(instance);
        }
    }
}
