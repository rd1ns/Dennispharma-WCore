using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record model
    /// </summary>
    public partial class UrlRecordModel : BaseWCoreEntityModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.System.SeNames.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.EntityId")]
        public int EntityId { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [WCoreResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }

        #endregion
    }
}