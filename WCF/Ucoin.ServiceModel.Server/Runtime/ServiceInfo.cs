using System;
using System.Runtime.Serialization;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [Serializable]
    [DataContract]
    public class ServiceInfo
    {
        [DataMember]
        public string BaseAddress { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public EndpointInfo[] EndpointsInfo { get; set; }

        [DataMember]
        public RunState State { get; set; }

        [DataMember]
        public int PackageId { get; set; }

    }
}
