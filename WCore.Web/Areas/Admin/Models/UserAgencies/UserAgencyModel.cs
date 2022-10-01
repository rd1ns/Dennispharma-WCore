using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Models.UserAgencies
{
    public class UserAgencyModel : BaseWCoreEntityModel
    {

        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Represents a UserAgency search model
    /// </summary>
    public partial class UserAgencySearchModel : BaseSearchModel
    {
        #region Ctor

        public UserAgencySearchModel()
        {
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Query")]
        public string Query { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }

        #endregion
    }
}
