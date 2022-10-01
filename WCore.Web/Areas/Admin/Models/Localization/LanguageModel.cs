using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Localization;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a language
    /// </summary>
    public partial class LanguageModel : BaseWCoreEntityModel
    {
        public LanguageModel()
        {
            Currencies = new List<SelectListItem>();
            LocaleResourceSearchModel = new LocaleResourceSearchModel();
        }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        public string LanguageCulture { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        public string UniqueSeoCode { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        public string FlagImageFileName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrencyId")]
        public int DefaultCurrencyId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsDefault")]
        public bool IsDefault { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsAdminDefault")]
        public bool IsAdminDefault { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Published")]
        public bool Published { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AllowSelection")]
        public bool AllowSelection { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }

        // search
        public LocaleResourceSearchModel LocaleResourceSearchModel { get; set; }

        public List<SelectListItem> Currencies { get; set; }
    }
}
