using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Ucoin.ServiceModel.Core
{
    /// <summary>
    /// From http://support.microsoft.com/kb/2538826
    /// </summary>
    public class WorkerThreadPoolSynchronizer : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            // WCF almost always uses Post
            ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            // Only the peer channel in WCF uses Send
            d(state);
        }
    }

    /// <summary>
    /// wcf第一次或第二次，可以达到最小线程数，或者线程数还没下来时能达到。但再次大并发请求时就从最小数量开始2个/s向上加。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WorkerThreadPoolBehaviorAttribute : Attribute, IContractBehavior
    {
        private static WorkerThreadPoolSynchronizer synchronizer = new WorkerThreadPoolSynchronizer();

        void IContractBehavior.AddBindingParameters(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
        }

        void IContractBehavior.ApplyClientBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
        }

        void IContractBehavior.ApplyDispatchBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.SynchronizationContext = synchronizer;
        }

        void IContractBehavior.Validate(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint)
        {
        }
    }
}
