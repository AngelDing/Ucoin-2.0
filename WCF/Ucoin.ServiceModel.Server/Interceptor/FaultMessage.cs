using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CtripSZ.ServiceModel.Server.Interceptor
{
    /// <summary>
    /// 异常消息数据实体
    /// </summary>
    [DataContractAttribute(Name = "FaultMessage", Namespace = Unity.Namespace)]
    public class FaultMessage : ExceptionDetail
    {
        public const string FaultSubCodeNamespace = Unity.Namespace + "/exceptionhandling/";
        public const string FaultSubCodeName = "ServiceError";
        public const string FaultAction = Unity.Namespace + "/fault";

        [DataMember]
        public string AssemblyQualifiedName { get; private set; }

        [DataMember]
        public new FaultMessage InnerException { get; private set; }


        public FaultMessage(Exception ex)
            : base(ex)
        {
            AssemblyQualifiedName = ex.GetType().AssemblyQualifiedName;
            if (null != ex.InnerException)
            {
                InnerException = new FaultMessage(ex.InnerException);
            }
        }

        public override string ToString()
        {
            return Message;
        }
    }
}

