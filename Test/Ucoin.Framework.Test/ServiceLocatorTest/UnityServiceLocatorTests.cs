using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Xunit;

namespace Ucoin.Framework.ServiceLocation.Test
{
    public class UnityServiceLocatorTests : ServiceLocatorTests
    {
        protected override IServiceLocator CreateServiceLocator()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<ILogger, AdvancedLogger>()
                .RegisterType<ILogger, SimpleLogger>(typeof(SimpleLogger).Name)
                .RegisterType<ILogger, AdvancedLogger>(typeof(AdvancedLogger).Name);

            //var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //IUnityContainer container = new UnityContainer();
            //container = section.Configure(container);

            return new UnityServiceLocator(container);
        }

        [Fact]
        public void GetInstance()
        {
            ILogger instance = locator.GetInstance<ILogger>();
            Assert.NotNull(instance);
        }

        [Fact]
        public void AskingForInvalidComponentShouldRaiseActivationException()
        {
            Assert.Throws<ActivationException>(delegate
            {
                locator.GetInstance<IDictionary>();
            });
        }

        [Fact]
        public void GetNamedInstance()
        {
            ILogger instance = locator.GetInstance<ILogger>(typeof(AdvancedLogger).Name);
            Assert.Same(typeof(AdvancedLogger), instance.GetType());
        }

        [Fact]
        public void GetNamedInstance2()
        {
            ILogger instance = locator.GetInstance<ILogger>(typeof(SimpleLogger).Name);
            Assert.Same(typeof(SimpleLogger), instance.GetType());
        }

        [Fact]
        public void GetUnknownInstance2()
        {
            Assert.Throws<ActivationException>(
                delegate
                {
                    locator.GetInstance<ILogger>("test");
                });
        }

        [Fact]
        public void GetAllInstances()
        {
            IEnumerable<ILogger> instances = locator.GetAllInstances<ILogger>();
            IList<ILogger> list = new List<ILogger>(instances);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void GetlAllInstance_ForUnknownType_ReturnEmptyEnumerable()
        {
            IEnumerable<IDictionary> instances = locator.GetAllInstances<IDictionary>();
            IList<IDictionary> list = new List<IDictionary>(instances);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void GenericOverload_GetInstance()
        {
            Assert.Equal(
                locator.GetInstance<ILogger>().GetType(),
                locator.GetInstance(typeof(ILogger), null).GetType());
        }

        [Fact]
        public void GenericOverload_GetInstance_WithName()
        {
            Assert.Equal(
                locator.GetInstance<ILogger>(typeof(AdvancedLogger).Name).GetType(),
                locator.GetInstance(typeof(ILogger), typeof(AdvancedLogger).Name).GetType()
            );
        }

        [Fact]
        public void Overload_GetInstance_NoName_And_NullName()
        {
            Assert.Equal(
                locator.GetInstance<ILogger>().GetType(),
                locator.GetInstance<ILogger>(null).GetType());
        }

        [Fact]
        public void GenericOverload_GetAllInstances()
        {
            List<ILogger> genericLoggers = new List<ILogger>(locator.GetAllInstances<ILogger>());
            List<object> plainLoggers = new List<object>(locator.GetAllInstances(typeof(ILogger)));
            Assert.Equal(genericLoggers.Count, plainLoggers.Count);
            for (int i = 0; i < genericLoggers.Count; i++)
            {
                Assert.Equal(
                    genericLoggers[i].GetType(),
                    plainLoggers[i].GetType());
            }
        }

        [Fact]
        public void UnityAdapter_Get_WithZeroLenName_ReturnsDefaultInstance()
        {
            Assert.Same(
                locator.GetInstance<ILogger>().GetType(),
                locator.GetInstance<ILogger>("").GetType()
            );
        }
    }
}
