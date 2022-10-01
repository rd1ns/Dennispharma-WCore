using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Caching;
using WCore.Core.Domain.Settings;

namespace WCore.Services.Configuration
{
    public class WCoreConfigurationDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new CacheKey("WCore.setting.all.dictionary.", WCoreEntityCacheDefaults<Setting>.Prefix);

        #endregion

        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "App_Data/appsettings.json";
    }
}
