using System;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;


namespace WCore.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item model
    /// </summary>
    public partial class ShoppingCartItemModel : BaseWCoreEntityModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.CurrentCarts.Store")]
        public string Store { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.Product")]
        public int ProductId { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.Product")]
        public string ProductName { get; set; }

        public string AttributeInfo { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.UnitPrice")]
        public string UnitPrice { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.Quantity")]
        public int Quantity { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.Total")]
        public string Total { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        #endregion
    }
}