using System;
using System.Collections.Generic;
using Ucoin.ServiceModel.Core;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
//using ProtoBuf.ServiceModel;

namespace Ucoin.ServiceModel.Core
{
    public sealed class DefaultEndpointPolicy : IEndpointPolicy
    {
        public static readonly IEndpointPolicy EndpointPolicy = new DefaultEndpointPolicy();

        private static readonly Lazy<Binding> _lazyBinding =
            new Lazy<Binding>(() =>
                {
                    return new CustomBinding("compressionBinding");
                    //return new NetTcpBinding("");
                });

        public Binding DefaultBinding
        {
            get { return _lazyBinding.Value; }
        }

        public string GetEndpointAddress(Type contract, string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                return contract.FullName;
            }
            return baseAddress.TrimEnd('/') + "/" + contract.FullName;
        }

        public ServiceEndpoint CreateServiceEndpoint(Type implementedContract, string basicAddress)
        {
            var address = new EndpointAddress(GetEndpointAddress(implementedContract, basicAddress));
            var serviceEndPoint = new ServiceEndpoint(ContractDescription.GetContract(implementedContract),
                DefaultBinding, address);
            ////替換WCF自帶序列化，使用Google的protobuf序列化
            //serviceEndPoint.Behaviors.Add(new ProtoEndpointBehavior());

            return serviceEndPoint;
        }
    }
}
