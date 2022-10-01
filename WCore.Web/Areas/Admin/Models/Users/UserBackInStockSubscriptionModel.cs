using System;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user back in stock subscription model
    /// </summary>
    public partial class UserBackInStockSubscriptionModel : BaseWCoreEntityModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Store")]
        public string StoreName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Product")]
        public int ProductId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Product")]
        public string ProductName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}