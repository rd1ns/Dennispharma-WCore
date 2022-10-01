using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Web.Areas.Admin.Models.UserAgencies;

namespace WCore.Web.Areas.Admin.Models.Users
{
    public class UserAgencyAuthorizationModel : BaseWCoreEntityModel
    {
        public UserAgencyAuthorizationModel()
        {
            UserAgencies = new List<SelectListItem>();
        }

        [WCoreResourceDisplayName("Admin.Configuration.IsRead")]
        public bool IsRead { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsCreate")]
        public bool IsCreate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsUpdate")]
        public bool IsUpdate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsDelete")]
        public bool IsDelete { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.UserAgency")]
        public int UserAgencyId { get; set; }
        public UserAgencyModel UserAgency { get; set; }

        public List<SelectListItem> UserAgencies { get; set; }
    }

    /// <summary>
    /// Represents a UserAgencyAuthorization search model
    /// </summary>
    public partial class UserAgencyAuthorizationSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserAgencyAuthorizationSearchModel()
        {
            UserAgencies = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.IsRead")]
        public bool? IsRead { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsCreate")]
        public bool? IsCreate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsUpdate")]
        public bool? IsUpdate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsDelete")]
        public bool? IsDelete { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.UserAgency")]
        public int? UserAgencyId { get; set; }

        public List<SelectListItem> UserAgencies { get; set; }
        #endregion
    }
}
