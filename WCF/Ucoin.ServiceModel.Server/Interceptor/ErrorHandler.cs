using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CtripSZ.ServiceModel.Server.Interceptor
{
    internal sealed class ErrorHandler : IErrorHandler
    {
        private readonly string _exceptionPolicyName;

        public ErrorHandler(string exceptionPolicyName)
        {
            if (string.IsNullOrEmpty(exceptionPolicyName))
            {
                throw new ArgumentNullException("exceptionPolicyName");
            }
            _exceptionPolicyName = exceptionPolicyName;
        }

        #region IErrorHandler Members

        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
            {
                return;
            }
            try
            {
                if (ExceptionPolicy.HandleException(error, _exceptionPolicyName))
                {
                    fault = Message.CreateMessage(version, BuildFault(error), FaultMessage.FaultAction);
                }
            }
            catch (Exception ex)
            {
                fault = Message.CreateMessage(version, BuildFault(ex), FaultMessage.FaultAction);
            }
        }

        private MessageFault BuildFault(Exception error)
        {
            var exceptionDetail = new FaultMessage(error);
            var code = FaultCode.CreateReceiverFaultCode(FaultMessage.FaultSubCodeName,
                FaultMessage.FaultSubCodeNamespace);
            return MessageFault.CreateFault(code,new FaultReason(error.Message), exceptionDetail);
        }

        #endregion
    }
}