using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucoin.Framework.Web.Resolver
{
    public class UnityMvcResolver : BaseResolver, IDependencyResolver
    {
        public UnityMvcResolver(IUnityContainer container)
            :base(container)
        {
        }    
    }
}