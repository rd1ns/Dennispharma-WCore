using System;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user order model
    /// </summary>
    public partial class UserOrderModel : BaseWCoreEntityModel
    {
        #region Properties

        public override int Id { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.OrderStatus")]
        public string OrderStatus { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.OrderStatus")]
        public int OrderStatusId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.PaymentStatus")]
        public string PaymentStatus { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.ShippingStatus")]
        public string ShippingStatus { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.OrderTotal")]
        public string OrderTotal { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.Store")]
        public string StoreName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.Orders.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}