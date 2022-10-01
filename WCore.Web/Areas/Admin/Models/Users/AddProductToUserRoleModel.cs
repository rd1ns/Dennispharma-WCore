using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a product model to add to the user role 
    /// </summary>
    public partial class AddProductToUserRoleModel : BaseWCoreEntityModel
    {
        #region Properties

        public int AssociatedToProductId { get; set; }

        #endregion
    }
}