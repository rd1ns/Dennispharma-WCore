using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using WCore.Core.Domain.Logging;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.Users;

namespace WCore.Web.Areas.Admin.Models.Logs
{
    public partial class LogModel : BaseWCoreEntityModel
    {
        #region Ctor
        public LogModel()
        {
            LogLevels = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.ShortMessage")]
        public string ShortMessage { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.FullMessage")]
        public string FullMessage { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IpAddress")]
        public string IpAddress { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.PageUrl")]
        public string PageUrl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ReferrerUrl")]
        public string ReferrerUrl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LogLevel")]
        public LogLevel LogLevel { get; set; }
        public string LogLevelName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.User")]
        public int? UserId { get; set; }
        public UserModel User { get; set; }

        public List<SelectListItem> LogLevels { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a Price search model
    /// </summary>
    public partial class LogSearchModel : BaseSearchModel
    {
        #region Ctor

        public LogSearchModel()
        {
            LogLevels = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.LogLevel")]
        public LogLevel? LogLevel { get; set; }

        public List<SelectListItem> LogLevels { get; set; }
        #endregion
    }
}
