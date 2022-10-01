using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a associated external auth records search model
    /// </summary>
    public class UserAssociatedExternalAuthRecordsSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth")]
        public IList<UserAssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; } = new List<UserAssociatedExternalAuthModel>();
        
        #endregion
    }
}
