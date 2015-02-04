using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace Ucoin.ServiceModel.Core
{
    public interface IEndpointPolicy
    {
        ServiceEndpoint CreateServiceEndpoint(Type implementedContract, string basicAddress);

        Binding DefaultBinding { get; }

        string GetEndpointAddress(Type contract, string baseAddress);
    }
}
