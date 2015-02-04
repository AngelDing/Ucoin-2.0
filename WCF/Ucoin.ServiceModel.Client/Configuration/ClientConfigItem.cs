
using Ucoin.ServiceModel.Core;
using System;
namespace Ucoin.ServiceModel.Client.Configuration
{
    public class ClientConfigItem
    {
        public string BaseAddress { get; set; }

        string _address = string.Empty;
        public string Address
        {
            set { _address = value; }
        }

        public string Assembly { get; set; }

        public string GetAddress<T>()
        {
            if (!string.IsNullOrEmpty(_address) && string.IsNullOrEmpty(BaseAddress))
            {
                return _address;
            }
            return GetAddress(typeof(T), BaseAddress);
        }

        private string GetAddress(Type type, string address)
        {
            return DefaultEndpointPolicy.EndpointPolicy.GetEndpointAddress(type, address);
        }
    }
}
