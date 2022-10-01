using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Orders;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart search model
    /// </summary>
    public partial class ShoppingCartSearchModel : BaseSearchModel
    {
        #region Ctor

        public ShoppingCartSearchModel()
        {
            AvailableShoppingCartTypes = new List<SelectListItem>();
            ShoppingCartItemSearchModel = new ShoppingCartItemSearchModel();
            AvailableStores = new List<SelectListItem>();
            AvailableCountries = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.ShoppingCartType.ShoppingCartType")]
        public ShoppingCartType ShoppingCartType { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.Product")]
        public int ProductId { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.BillingCountry")]
        public int BillingCountryId { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.Store")]
        public int StoreId { get; set; }

        public IList<SelectListItem> AvailableShoppingCartTypes { get; set; }

        public ShoppingCartItemSearchModel ShoppingCartItemSearchModel { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool HideStoresList { get; set; }

        #endregion
    }
}