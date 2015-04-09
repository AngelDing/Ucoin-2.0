using Ucoin.Framework.ServiceLocation;
using Xunit;
using FluentAssertions;
using System;

namespace Ucoin.Framework.ServiceLocation.Test
{
    public class ServiceLocatorFixtureTests
    {
        public ServiceLocatorFixtureTests()
        {
            ServiceLocator.SetLocatorProvider(null);
        }

        [Fact]
        public void ServiceLocatorIsLocationProviderSetReturnsTrueWhenSet()
        {
            ServiceLocator.SetLocatorProvider(() => new MockServiceLocator());
            ServiceLocator.IsLocationProviderSet.Should().BeTrue();
        }

        [Fact]
        public void ServiceLocatorIsLocationProviderSetReturnsFalseWhenNotSet()
        {
            ServiceLocator.IsLocationProviderSet.Should().BeFalse();
        }

        [Fact]
        public void ServiceLocatorCurrentThrowsWhenLocationProviderNotSet()
        {
            Assert.Throws<InvalidOperationException>(delegate
            {
                var currentServiceLocator = ServiceLocator.Current;
            });
        }
    }
}
