
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
namespace Ucoin.ServiceModel.Core
{
    public class ServiceScope : IDisposable
    {
        private static ThreadLocal<ServiceScope> _currentScope = new ThreadLocal<ServiceScope>(() => null);

        public object ServiceInfo { get; set; }

        public static ServiceScope Current
        {
            get
            {
                return _currentScope.Value;
            }
            set
            {
                _currentScope.Value = value;
            }
        }

        /// <summary>
        /// Dispose实例
        /// </summary>
        public void Dispose()
        {
            CloseChannel(ServiceInfo as IClientChannel);
            _currentScope.Value = null;
            GC.SuppressFinalize(this);
        }


        private void CloseChannel(IClientChannel val)
        {
            if (val == null)
            {
                return;
            }
            try
            {
                if (val.State == CommunicationState.Faulted)
                {
                    val.Abort();
                }
                else
                {
                    val.Close();
                }
            }
            catch (ObjectDisposedException)
            {
                //TODO
            }
        }       
    }
}