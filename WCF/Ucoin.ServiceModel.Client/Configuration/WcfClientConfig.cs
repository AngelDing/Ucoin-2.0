using System;

namespace Ucoin.ServiceModel.Client.Configuration
{
    public static class WcfClientConfig
    {
        static readonly Lazy<WcfClientSection> _instance = new Lazy<WcfClientSection>(() =>
        {
            return WcfClientSection.Instance;
        });

        public static WcfClientSection Current
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}
