using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a display default footer item settings model
    /// </summary>
    public partial class DisplayDefaultFooterItemSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplaySitemapFooterItem")]
        public bool DisplaySitemapFooterItem { get; set; }
        public bool DisplaySitemapFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayContactUsFooterItem")]
        public bool DisplayContactUsFooterItem { get; set; }
        public bool DisplayContactUsFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayProductSearchFooterItem")]
        public bool DisplayProductSearchFooterItem { get; set; }
        public bool DisplayProductSearchFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewsFooterItem")]
        public bool DisplayNewsFooterItem { get; set; }
        public bool DisplayNewsFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayBlogFooterItem")]
        public bool DisplayBlogFooterItem { get; set; }
        public bool DisplayBlogFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayForumsFooterItem")]
        public bool DisplayForumsFooterItem { get; set; }
        public bool DisplayForumsFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayRecentlyViewedProductsFooterItem")]
        public bool DisplayRecentlyViewedProductsFooterItem { get; set; }
        public bool DisplayRecentlyViewedProductsFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCompareProductsFooterItem")]
        public bool DisplayCompareProductsFooterItem { get; set; }
        public bool DisplayCompareProductsFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewProductsFooterItem")]
        public bool DisplayNewProductsFooterItem { get; set; }
        public bool DisplayNewProductsFooterItem_OverrideForStore { get; set; }       

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayUserInfoFooterItem")]
        public bool DisplayUserInfoFooterItem { get; set; }
        public bool DisplayUserInfoFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayUserOrdersFooterItem")]
        public bool DisplayUserOrdersFooterItem { get; set; }
        public bool DisplayUserOrdersFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayUserAddressesFooterItem")]
        public bool DisplayUserAddressesFooterItem { get; set; }
        public bool DisplayUserAddressesFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayShoppingCartFooterItem")]
        public bool DisplayShoppingCartFooterItem { get; set; }
        public bool DisplayShoppingCartFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayWishlistFooterItem")]
        public bool DisplayWishlistFooterItem { get; set; }
        public bool DisplayWishlistFooterItem_OverrideForStore { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayApplyVendorAccountFooterItem")]
        public bool DisplayApplyVendorAccountFooterItem { get; set; }
        public bool DisplayApplyVendorAccountFooterItem_OverrideForStore { get; set; }

        #endregion
    }
}