using System;
using System.Reflection;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public class DomainAgent : MarshalByRefObject
    {
        public IServiceRunner CreateRunner( )
        {
            return new RemoteServiceRunner();
        }

        public static DomainAgent CreateInstance(AppDomain targetDomain)
        {
            var oh = targetDomain.CreateInstance(
                Assembly.GetExecutingAssembly().FullName,
                typeof(DomainAgent).FullName
            );
            var obj = oh.Unwrap();
            return (DomainAgent)obj;
        }
    }
}
