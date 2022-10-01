using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a SEO settings model
    /// </summary>
    public partial class SeoSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Properties
        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.PageTitleSeparator")]
        [NoTrim]
        public string PageTitleSeparator { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.PageTitleSeoAdjustment")]
        public int PageTitleSeoAdjustment { get; set; }
        public SelectList PageTitleSeoAdjustmentValues { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.DefaultTitle")]
        public string DefaultTitle { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.DefaultMetaKeywords")]
        public string DefaultMetaKeywords { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.DefaultMetaDescription")]
        public string DefaultMetaDescription { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.ReservedUrlRecordSlugs")]
        public string ReservedUrlRecordSlugs { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.GenerateProductMetaDescription")]
        public bool GenerateProductMetaDescription { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.ConvertNonWesternChars")]
        public bool ConvertNonWesternChars { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.CanonicalUrlsEnabled")]
        public bool CanonicalUrlsEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.WwwRequirement")]
        public int WwwRequirement { get; set; }
        public SelectList WwwRequirementValues { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.TwitterMetaTags")]
        public bool TwitterMetaTags { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.OpenGraphMetaTags")]
        public bool OpenGraphMetaTags { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.CustomHeadTags")]
        public string CustomHeadTags { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.SeoSetting.Microdata")]
        public bool MicrodataEnabled { get; set; }
        #endregion
    }
}
