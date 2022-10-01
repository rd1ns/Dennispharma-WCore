using System;
using WCore.Core.Domain.Orders;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item search model
    /// </summary>
    public partial class ShoppingCartItemSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        public ShoppingCartType ShoppingCartType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int ProductId { get; set; }

        public int BillingCountryId { get; set; }

        public int StoreId { get; set; }

        #endregion
    }
}