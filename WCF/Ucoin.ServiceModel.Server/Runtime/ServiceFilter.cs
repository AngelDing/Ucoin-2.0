using System;

namespace CtripSZ.ServiceModel.Runtime
{
    [Serializable]
    public abstract class ServiceFilter:IServiceFilter
    {
        
        public static readonly ServiceFilter Empty = new EmptyFilter();
    
        public bool IsEmpty
        {
            get { return this is ServiceFilter.EmptyFilter; }
        }
        
        public virtual bool Pass(IService test)
        {
            return Match(test);
        }
                 
        public abstract bool Match(IService test);
                
        [Serializable]
        private class EmptyFilter : ServiceFilter
        {
            public override bool Match(IService test)
            {
                return true;
            }

            public override bool Pass(IService test)
            {
                return true;
            }
        }
    }
}
