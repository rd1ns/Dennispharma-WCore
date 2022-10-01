using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a date time settings model
    /// </summary>
    public partial class DateTimeSettingsModel : BaseWCoreModel, ISettingsModel
    {
        #region Ctor

        public DateTimeSettingsModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToSetTimeZone")]
        public bool AllowUsersToSetTimeZone { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultStoreTimeZone")]
        public string DefaultStoreTimeZoneId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultStoreTimeZone")]
        public IList<SelectListItem> AvailableTimeZones { get; set; }

        #endregion
    }
}