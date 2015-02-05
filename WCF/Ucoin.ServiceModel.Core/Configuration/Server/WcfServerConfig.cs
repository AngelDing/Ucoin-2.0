using System;
using System.IO;

namespace CtripSZ.ServiceModel.Configuration.Server
{
    partial class WcfServerSection
    {
        private static readonly Lazy<WcfServerSection> _val = new Lazy<WcfServerSection>(() =>
        {
            var file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "servicemodel.config");
            if (File.Exists(file))
            {
                var cfg = ConfigManager.Create(file);
                return cfg.GetSection("wcf.server") as WcfServerSection;
            }

            return WcfServerSection.Instance;
        });

        public static WcfServerSection Current
        {
            get { return _val.Value; }
        }
    }
}
