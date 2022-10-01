using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user shopping cart search model
    /// </summary>
    public partial class UserShoppingCartSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserShoppingCartSearchModel()
        {
            AvailableShoppingCartTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        [WCoreResourceDisplayName("Admin.ShoppingCartType.ShoppingCartType")]
        public int ShoppingCartTypeId { get; set; }

        public IList<SelectListItem> AvailableShoppingCartTypes { get; set; }

        #endregion
    }
}