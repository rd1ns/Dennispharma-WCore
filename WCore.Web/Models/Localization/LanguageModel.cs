using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Localization;
using System.Collections.Generic;

namespace WCore.Web.Models.Localization
{
    public partial class LanguageModel : BaseWCoreEntityModel
    {
        #region Ctor
        public LanguageModel()
        {

        }
        #endregion
        #region Properties
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public string UniqueSeoCode { get; set; }
        public string FlagImageFileName { get; set; }
        public bool Rtl { get; set; }
        public int DefaultCurrencyId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAdminDefault { get; set; }
        public bool Published { get; set; }
        public bool AllowSelection { get; set; }
        public int DisplayOrder { get; set; } 
        #endregion
    }
}
