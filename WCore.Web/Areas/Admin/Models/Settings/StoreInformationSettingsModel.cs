using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Configuration;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Store information settings
    /// </summary>
    public class StoreInformationSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Ctor
        public StoreInformationSettingsModel()
        {
            ThemeLayoutTypes = new List<SelectListItem>();
            ThemeColorSchemes = new List<SelectListItem>();
            ThemeHeaderTypes = new List<SelectListItem>();
            ThemePageTitles = new List<SelectListItem>();
            ThemeGalleryTypes = new List<SelectListItem>();
            ThemeTemplateFeatures = new List<SelectListItem>();
            ThemeTemplateTypes = new List<SelectListItem>();
            Themes = new List<SelectListItem>();
            AvailableStoreThemes = new List<ThemeModel>();
            Countries = new List<SelectListItem>();
            Languages = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.StoreClosed")]
        public bool StoreClosed { get; set; }
        public bool StoreClosed_OverrideForStore { get; set; }
        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.LogoPicture")]
        public string LogoPicture { get; set; }
        public bool LogoPicture_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.StickyPicture")]
        public string StickyPicture { get; set; }
        public bool StickyPicture_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.BgPattern")]
        public string BgPattern { get; set; }
        public bool BgPattern_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.BgImage")]
        public string BgImage { get; set; }
        public bool BgImage_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.PageTitleImage")]
        public string PageTitleImage { get; set; }
        public bool PageTitleImage_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreTheme")]
        public string DefaultStoreTheme { get; set; }
        public bool DefaultStoreTheme_OverrideForStore { get; set; }
        public IList<ThemeModel> AvailableStoreThemes { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeTemplateFeature")]
        public string DefaultStoreThemeTemplateFeature { get; set; }
        public bool DefaultStoreThemeTemplateFeature_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeTemplateType")]
        public string DefaultStoreThemeTemplateType { get; set; }
        public bool DefaultStoreThemeTemplateType_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeLayoutType")]
        public string DefaultStoreThemeLayoutType { get; set; }
        public bool DefaultStoreThemeLayoutType_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme color scheme
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeColorScheme")]
        public string DefaultStoreThemeColorScheme { get; set; }
        public bool DefaultStoreThemeColorScheme_OverrideForStore { get; set; }


        /// <summary>
        /// Gets or sets a default store theme header type
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeHeaderType")]
        public string DefaultStoreThemeHeaderType { get; set; }
        public bool DefaultStoreThemeHeaderType_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme page title
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemePageTitle")]
        public string DefaultStoreThemePageTitle { get; set; }
        public bool DefaultStoreThemePageTitle_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a default store theme gallery type
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DefaultStoreThemeGalleryType")]
        public string DefaultStoreThemeGalleryType { get; set; }
        public bool DefaultStoreThemeGalleryType_OverrideForStore { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select a theme
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.AllowUserToSelectTheme")]
        public bool AllowUserToSelectTheme { get; set; }
        public bool AllowUserToSelectTheme_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should display warnings about the new EU cookie law
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.DisplayEuCookieLawWarning")]
        public bool DisplayEuCookieLawWarning { get; set; }
        public bool DisplayEuCookieLawWarning_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value of Facebook page URL of the site
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.FacebookLink")]
        public string FacebookLink { get; set; }
        public bool FacebookLink_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value of Twitter page URL of the site
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.TwitterLink")]
        public string TwitterLink { get; set; }
        public bool TwitterLink_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.YoutubeLink")]
        public string YoutubeLink { get; set; }
        public bool YoutubeLink_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.InstagramLink")]
        public string InstagramLink { get; set; }
        public bool InstagramLink_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        [WCoreResourceDisplayName("Admin.Configuration.Settings.StoreInformation.LinkedinLink")]
        public string LinkedinLink { get; set; }
        public bool LinkedinLink_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SubjectFieldOnContactUsForm")]
        public bool SubjectFieldOnContactUsForm { get; set; }
        public bool SubjectFieldOnContactUsForm_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseSystemEmailForContactUsForm")]
        public bool UseSystemEmailForContactUsForm { get; set; }
        public bool UseSystemEmailForContactUsForm_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PopupForTermsOfServiceLinks")]
        public bool PopupForTermsOfServiceLinks { get; set; }
        public bool PopupForTermsOfServiceLinks_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultLanguageId")]
        public int DefaultLanguageId { get; set; }
        public int DefaultLanguageId_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultCountryId")]
        public int DefaultCountryId { get; set; }
        public int DefaultCountryId_OverrideForStore { get; set; }

        public List<SelectListItem> Themes { get; set; }
        public List<SelectListItem> ThemeLayoutTypes { get; set; }
        public List<SelectListItem> ThemeColorSchemes { get; set; }
        public List<SelectListItem> ThemeHeaderTypes { get; set; }
        public List<SelectListItem> ThemePageTitles { get; set; }
        public List<SelectListItem> ThemeGalleryTypes { get; set; }
        public List<SelectListItem> ThemeTemplateFeatures { get; set; }
        public List<SelectListItem> ThemeTemplateTypes { get; set; }
        public List<SelectListItem> Languages { get; set; }
        public List<SelectListItem> Countries { get; set; }

        #endregion

        #region Nested classes

        public partial class ThemeModel
        {
            public string SystemName { get; set; }
            public string FriendlyName { get; set; }
            public string PreviewImageUrl { get; set; }
            public string PreviewText { get; set; }
            public bool SupportRtl { get; set; }
            public bool Selected { get; set; }
        }

        #endregion
    }
}
