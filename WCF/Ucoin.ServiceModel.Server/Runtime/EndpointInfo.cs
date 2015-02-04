using System;
using System.Runtime.Serialization;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [DataContract]
    [Serializable]
    public class EndpointInfo
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Binding { get; set; }

        [DataMember]
        public string ContractName { get; set; }
    }
}
