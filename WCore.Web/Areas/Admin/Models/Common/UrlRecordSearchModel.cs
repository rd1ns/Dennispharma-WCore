using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WCore.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record search model
    /// </summary>
    public partial class UrlRecordSearchModel : BaseSearchModel
    {
        #region Ctor

        public UrlRecordSearchModel()
        {
            AvailableLanguages = new List<SelectListItem>();
            AvailableActiveOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.System.SeNames.List.Name")]
        public string SeName { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.List.Language")]
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.List.IsActive")]
        public int IsActiveId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        public IList<SelectListItem> AvailableActiveOptions { get; set; }

        #endregion
    }
}