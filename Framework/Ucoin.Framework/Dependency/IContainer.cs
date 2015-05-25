using System;

namespace Ucoin.Framework.Dependency
{
    public interface IContainer
    {
        void Register<TService>(Func<TService> factory);

        TService Resolve<TService>();
    }
}