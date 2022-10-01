﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review search model
    /// </summary>
    public partial class ProductReviewSearchModel : BaseSearchModel
    {
        #region Ctor

        public ProductReviewSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.SearchText")]
        public string SearchText { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.SearchStore")]
        public int SearchStoreId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.SearchProduct")]
        public int SearchProductId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviews.List.SearchApproved")]
        public int SearchApprovedId { get; set; }

        //vendor
        public bool IsLoggedInAsVendor { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }

        public bool HideStoresList { get; set; }

        #endregion
    }
}