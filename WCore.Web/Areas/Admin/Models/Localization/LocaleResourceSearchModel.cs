using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource search model
    /// </summary>
    public partial class LocaleResourceSearchModel : BaseSearchModel
    {
        #region Ctor

        public LocaleResourceSearchModel()
        {
            AddResourceString = new LocaleResourceModel();
        }

        #endregion

        #region Properties

        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceName")]
        public string SearchResourceName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceValue")]
        public string SearchResourceValue { get; set; }

        public LocaleResourceModel AddResourceString { get; set; }

        #endregion
    }
}
