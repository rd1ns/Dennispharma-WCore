using WCore.Core.Configuration;

namespace WCore.Core.Domain.Settings
{
    /// <summary>
    /// Store information settings
    /// </summary>
    public class StoreInformationSettings : ISettings
    {

        /// <summary>
        /// Gets or sets a value indicating whether store is closed
        /// </summary>
        public bool StoreClosed { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string LogoPicture { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string StickyPicture { get; set; }

        /// <summary>
        /// Gets or sets a default store theme
        /// </summary>
        public string DefaultStoreTheme { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        public string DefaultStoreThemeLayoutType { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        public string DefaultStoreThemeTemplateType { get; set; }

        /// <summary>
        /// Gets or sets a default store theme layout type
        /// </summary>
        public string DefaultStoreThemeTemplateFeature { get; set; }

        /// <summary>
        /// Gets or sets a default store theme color scheme
        /// </summary>
        public string DefaultStoreThemeColorScheme { get; set; }

        /// <summary>
        /// Gets or sets a default store theme header type
        /// </summary>
        public string DefaultStoreThemeHeaderType { get; set; }

        /// <summary>
        /// Gets or sets a default store theme page title
        /// </summary>
        public string DefaultStoreThemePageTitle { get; set; }

        /// <summary>
        /// Gets or sets a default store theme gallery type
        /// </summary>
        public string DefaultStoreThemeGalleryType { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string BgPattern { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string BgImage { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string PageTitleImage { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select a theme
        /// </summary>
        public bool AllowUserToSelectTheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should display warnings about the new EU cookie law
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        /// <summary>
        /// Gets or sets a value of Facebook page URL of the site
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Twitter page URL of the site
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        public string YoutubeLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Instagram page URL of the site
        /// </summary>
        public string InstagramLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Linkedin page URL of the site
        /// </summary>
        public string LinkedinLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Google+ page URL of the site
        /// </summary>
        public string FormsForwardingMail { get; set; }

        /// <summary>
        /// Gets or sets a value of default language
        /// </summary>
        public int DefaultLanguageId { get; set; }

        /// <summary>
        /// Gets or sets a value of default country
        /// </summary>
        public int DefaultCountryId { get; set; }
    }
}
