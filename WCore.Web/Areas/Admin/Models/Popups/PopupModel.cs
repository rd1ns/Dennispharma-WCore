using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Popup;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.DynamicForms;
using WCore.Web.Areas.Admin.Models.Galleries;

namespace WCore.Web.Areas.Admin.Models.Popups
{
    public partial class PopupModel : BaseWCoreEntityModel, ILocalizedModel<PopupLocalizedModel>
    {
        #region Ctor
        public PopupModel()
        {
            Locales = new List<PopupLocalizedModel>();
            PopupShowTypes = new List<SelectListItem>();
            PopupTimeTypes = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowUrl")]
        public string ShowUrl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PopupTime")]
        public int PopupTime { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PopupShowType")]
        public PopupShowType PopupShowType { get; set; }
        public string PopupShowTypeName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PopupTimeType")]
        public PopupTimeType PopupTimeType { get; set; }
        public string PopupTimeTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowHeader")]
        public bool ShowHeader { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowFooter")]
        public bool ShowFooter { get; set; }
        public IList<PopupLocalizedModel> Locales { get; set; }
        public List<SelectListItem> PopupShowTypes { get; set; }
        public List<SelectListItem> PopupTimeTypes { get; set; }

        #endregion
    }

    public partial class PopupLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
    }
    public partial class PopupSearchModel : BaseSearchModel
    {
        #region Ctor

        public PopupSearchModel()
        {
            PopupShowTypes = new List<SelectListItem>();
            PopupTimeTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.ShowUrl")]
        public string ShowUrl { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        public List<SelectListItem> PopupShowTypes { get; set; }
        public List<SelectListItem> PopupTimeTypes { get; set; }
        #endregion
    }
}
