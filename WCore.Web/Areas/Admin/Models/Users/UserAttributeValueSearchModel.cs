using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user attribute value search model
    /// </summary>
    public partial class UserAttributeValueSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserAttributeId { get; set; }

        #endregion
    }
}