using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Models.Vendors;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a vendor settings model
    /// </summary>
    public partial class VendorSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Ctor

        public VendorSettingsModel()
        {
            VendorAttributeSearchModel = new VendorAttributeSearchModel();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.VendorsBlockItemsToDisplay")]
        public int VendorsBlockItemsToDisplay { get; set; }
        public bool VendorsBlockItemsToDisplay_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.ShowVendorOnProductDetailsPage")]
        public bool ShowVendorOnProductDetailsPage { get; set; }
        public bool ShowVendorOnProductDetailsPage_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowUsersToContactVendors")]
        public bool AllowUsersToContactVendors { get; set; }
        public bool AllowUsersToContactVendors_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowUsersToApplyForVendorAccount")]
        public bool AllowUsersToApplyForVendorAccount { get; set; }
        public bool AllowUsersToApplyForVendorAccount_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.TermsOfServiceEnabled")]
        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServiceEnabled_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowSearchByVendor")]
        public bool AllowSearchByVendor { get; set; }
        public bool AllowSearchByVendor_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowVendorsToEditInfo")]
        public bool AllowVendorsToEditInfo { get; set; }
        public bool AllowVendorsToEditInfo_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.NotifyStoreOwnerAboutVendorInformationChange")]
        public bool NotifyStoreOwnerAboutVendorInformationChange { get; set; }
        public bool NotifyStoreOwnerAboutVendorInformationChange_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.MaximumProductNumber")]
        public int MaximumProductNumber { get; set; }
        public bool MaximumProductNumber_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowVendorsToImportProducts")]
        public bool AllowVendorsToImportProducts { get; set; }
        public bool AllowVendorsToImportProducts_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.Vendor.ShowVendorOnOrderDetailsPage")]
        public bool ShowVendorOnOrderDetailsPage { get; set; }
        public bool ShowVendorOnOrderDetailsPage_OverrideForStore { get; set; }

        public VendorAttributeSearchModel VendorAttributeSearchModel { get; set; }

        #endregion
    }
}