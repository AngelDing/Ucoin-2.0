using System;

namespace Ucoin.Framework.Web
{
    using System;
    using System.Configuration;

    public class MaintenanceMode
    {
        public const string MaintenanceModeSettingName = "MaintenanceMode";

        public static bool IsInMaintainanceMode { get; internal set; }

        public static void RefreshIsInMaintainanceMode()
        {
            var setting = ConfigurationManager.GetSection(MaintenanceModeSettingName);
            if (setting == null)
            {
                IsInMaintainanceMode = false;
                return;
            }
            var settingValue = setting.ToString();

            IsInMaintainanceMode = (!string.IsNullOrEmpty(settingValue) &&
                                    string.Equals(settingValue, "true", StringComparison.OrdinalIgnoreCase));
        }
    }
}
