using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Common;

namespace WCore.Web.Areas.Admin.Models.Stores
{
    /// <summary>
    /// Represents a store model
    /// </summary>
    public partial class StoreModel : BaseWCoreEntityModel, ILocalizedModel<StoreLocalizedModel>
    {
        #region Ctor

        public StoreModel()
        {
            Locales = new List<StoreLocalizedModel>();
            AvailableLanguages = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Stores.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.Url")]
        public string Url { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.SslEnabled")]
        public virtual bool SslEnabled { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.Hosts")]
        public string Hosts { get; set; }

        //default language
        [WCoreResourceDisplayName("Admin.Stores.Fields.DefaultLanguage")]
        public int DefaultLanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.CompanyName")]
        public string CompanyName { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.CompanyAddress")]
        public string CompanyAddress { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.CompanyPhoneNumber")]
        public string CompanyPhoneNumber { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.CompanyVat")]
        public string CompanyVat { get; set; }

        public IList<StoreLocalizedModel> Locales { get; set; }

        #endregion
    }
    public partial class StoreLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Stores.Fields.Name")]
        public string Name { get; set; }
    }
    public partial class StoreSearchModel : BaseSearchModel
    {
        #region Ctor

        public StoreSearchModel()
        {
        }

        #endregion
    }
    public partial class StoreListModel : BasePagedListModel<StoreModel>
    {
    }
}
