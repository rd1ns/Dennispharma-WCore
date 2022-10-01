using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor model
    /// </summary>
    public partial class VendorAttributeValueModel : BaseWCoreEntityModel, ILocalizedModel<VendorAttributeValueLocalizedModel>
    {
        #region Ctor
        public VendorAttributeValueModel()
        {
            Locales = new List<VendorAttributeValueLocalizedModel>();
        }
        #endregion

        #region Properties

        public int VendorAttributeId { get; set; }
        public VendorAttributeModel VendorAttribute { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<VendorAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }
    public partial class VendorAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }
    }
    public partial class VendorAttributeValueSearchModel : BaseSearchModel
    {
        #region Ctor

        public VendorAttributeValueSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public int VendorAttributeId { get; set; }
        public virtual VendorAttributeValueModel VendorAttribute { get; set; }

        #endregion
    }
    public partial class VendorAttributeValueListModel : BasePagedListModel<VendorAttributeValueModel>
    {
    }
}
