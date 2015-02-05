using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace CtripSZ.ServiceModel.Server.Interceptor
{
    internal class ClientMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            OnReceive(ref reply);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }

        private void OnReceive(ref Message reply)
        {
            if (reply.IsFault && reply.Headers.Action == FaultMessage.FaultAction)
            {
                var fault = MessageFault.CreateFault(reply, int.MaxValue);
                if (fault.Code.SubCode.Name == FaultMessage.FaultSubCodeName &&
                    fault.Code.SubCode.Namespace == FaultMessage.FaultSubCodeNamespace)
                {
                    var exception = (FaultException<FaultMessage>)FaultException
                        .CreateFault(fault, typeof(FaultMessage));

                    throw GetException(exception.Detail);
                }
            }
        }

        private Exception GetException(FaultMessage exceptionDetail)
        {
            if (null == exceptionDetail.InnerException)
            {
                return (Exception)Activator.CreateInstance(
                    Type.GetType(exceptionDetail.AssemblyQualifiedName),
                    exceptionDetail.Message);
            }

            var innerException = GetException(exceptionDetail.InnerException);
            return (Exception)Activator.CreateInstance(
                Type.GetType(exceptionDetail.AssemblyQualifiedName),
                exceptionDetail.Message, innerException);
        }
    }
}
