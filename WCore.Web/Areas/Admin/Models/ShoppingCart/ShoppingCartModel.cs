using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart model
    /// </summary>
    public partial class ShoppingCartModel : BaseWCoreModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.CurrentCarts.User")]
        public int UserId { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.User")]
        public string UserEmail { get; set; }

        [WCoreResourceDisplayName("Admin.CurrentCarts.TotalItems")]
        public int TotalItems { get; set; }

        #endregion
    }
}