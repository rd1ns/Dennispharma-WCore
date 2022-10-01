using WCore.Web.Areas.Admin.Models.ShoppingCart;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user shopping cart list model
    /// </summary>
    public partial class UserShoppingCartListModel : BasePagedListModel<ShoppingCartItemModel>
    {
    }
}