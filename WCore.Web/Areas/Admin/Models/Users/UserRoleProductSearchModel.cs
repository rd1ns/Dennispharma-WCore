using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user role product search model
    /// </summary>
    public partial class UserRoleProductSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserRoleProductSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
            AddProductToUserRoleModel = new AddProductToUserRoleModel();
        }

        #endregion

        #region Properties

        public AddProductToUserRoleModel AddProductToUserRoleModel { get; set; }

        public bool IsLoggedInAsVendor { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
        public string SearchProductName { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
        public int SearchStoreId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
        public int SearchVendorId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
        public int SearchProductTypeId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableProductTypes { get; set; }

        #endregion
    }
}