using System;
using System.Runtime.ConstrainedExecution;

namespace Ucoin.Framework
{
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        protected abstract void Dispose(bool disposing);

        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
       
        public void Dispose()
        {
            this.ExplicitDispose();
        }
    }
}
