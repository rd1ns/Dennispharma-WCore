using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Catalog;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Common;

namespace WCore.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor model
    /// </summary>
    public partial class VendorModel : BaseWCoreEntityModel, ILocalizedModel<VendorLocalizedModel>
    {
        #region Ctor
        public VendorModel()
        {
            Address = new AddressModel();
            VendorAttributes = new List<VendorAttributeModel>();
            Locales = new List<VendorLocalizedModel>();
            AssociatedUsers = new List<VendorAssociatedUserModel>();
            VendorNoteSearchModel = new VendorNoteSearchModel();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [WCoreResourceDisplayName("Admin.Configuration.Email")]
        public string Email { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AdminComment")]
        public string AdminComment { get; set; }

        public AddressModel Address { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.PageSize")]
        public int PageSize { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AllowUsersToSelectPageSize")]
        public bool AllowUsersToSelectPageSize { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        public List<VendorAttributeModel> VendorAttributes { get; set; }

        public IList<VendorLocalizedModel> Locales { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AssociatedUserEmails")]
        public IList<VendorAssociatedUserModel> AssociatedUsers { get; set; }

        //vendor notes
        [WCoreResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string AddVendorNoteMessage { get; set; }

        public VendorNoteSearchModel VendorNoteSearchModel { get; set; }

        #endregion

        #region Nested classes

        public partial class VendorAttributeModel : BaseWCoreEntityModel
        {
            public VendorAttributeModel()
            {
                Values = new List<VendorAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<VendorAttributeValueModel> Values { get; set; }
        }

        public partial class VendorAttributeValueModel : BaseWCoreEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
    public partial class VendorLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }
    public partial class VendorSearchModel : BaseSearchModel
    {
        #region Ctor

        public VendorSearchModel()
        {
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [WCoreResourceDisplayName("Admin.Configuration.Email")]
        public string Email { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        #endregion
    }
    public partial class VendorListModel : BasePagedListModel<VendorModel>
    {
    }
}
