using System;
using System.ServiceModel.Configuration;

namespace Ucoin.ServiceModel.Core
{
    public class ServiceInterceptorBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(ServiceInterceptorAttribute); }
        }

        protected override object CreateBehavior()
        {
            return new ServiceInterceptorAttribute();
        }
    }
}
