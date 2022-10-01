using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Configuration;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Common settings
    /// </summary>
    public class LocalizationSettingsModel : BaseWCoreModel, ISettingsModel
    {
        public LocalizationSettingsModel()
        {
            Languages = new List<SelectListItem>();
        }
        /// <summary>
        /// Default admin area language identifier
        /// </summary>

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.DefaultAdminLanguageId")]
        public int DefaultAdminLanguageId { get; set; }

        /// <summary>
        /// Use images for language selection
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.UseImagesForLanguageSelection")]
        public bool UseImagesForLanguageSelection { get; set; }

        /// <summary>
        /// A value indicating whether SEO friendly URLs with multiple languages are enabled
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.SeoFriendlyUrlsForLanguagesEnabled")]
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        /// <summary>
        /// A value indicating whether we should detect the current language by a user region (browser settings)
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.AutomaticallyDetectLanguage")]
        public bool AutomaticallyDetectLanguage { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocaleStringResource records on application startup
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.LoadAllLocaleRecordsOnStartup")]
        public bool LoadAllLocaleRecordsOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocalizedProperty records on application startup
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.LoadAllLocalizedPropertiesOnStartup")]
        public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to load all search engine friendly names (slugs) on application startup
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.LoadAllUrlRecordsOnStartup")]
        public bool LoadAllUrlRecordsOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to we should ignore RTL language property for admin area.
        /// It's useful for store owners with RTL languages for public store but preferring LTR for admin area
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.Localization.IgnoreRtlPropertyForAdminArea")]
        public bool IgnoreRtlPropertyForAdminArea { get; set; }


        public List<SelectListItem> Languages { get; set; }
    }
}
