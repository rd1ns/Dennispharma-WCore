using System;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user activity log model
    /// </summary>
    public partial class UserActivityLogModel : BaseWCoreEntityModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.Users.Users.ActivityLog.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.ActivityLog.Comment")]
        public string Comment { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.ActivityLog.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [WCoreResourceDisplayName("Admin.Users.Users.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }

        #endregion
    }
}