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
    public partial class VendorAttributeModel : BaseWCoreEntityModel, ILocalizedModel<VendorAttributeLocalizedModel>
    {
        #region Ctor
        public VendorAttributeModel()
        {
            Locales = new List<VendorAttributeLocalizedModel>();
            VendorAttributeValueSearchModel = new VendorAttributeValueSearchModel();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<VendorAttributeLocalizedModel> Locales { get; set; }

        public VendorAttributeValueSearchModel VendorAttributeValueSearchModel { get; set; }

        #endregion

        #region Nested classes

        public partial class VendorAttributeModelAttributeModel : BaseWCoreEntityModel
        {
            public VendorAttributeModelAttributeModel()
            {
                Values = new List<VendorAttributeModelAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<VendorAttributeModelAttributeValueModel> Values { get; set; }
        }

        public partial class VendorAttributeModelAttributeValueModel : BaseWCoreEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
    public partial class VendorAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }
    }
    public partial class VendorAttributeSearchModel : BaseSearchModel
    {
        #region Ctor

        public VendorAttributeSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public int VendorAttributeId { get; set; }
        public virtual VendorAttributeModel VendorAttribute { get; set; }

        #endregion
    }
    public partial class VendorAttributeListModel : BasePagedListModel<VendorAttributeModel>
    {
    }
}
