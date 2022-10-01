using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Teams
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class TeamModel : BaseWCoreEntityModel
    {

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Surname")]
        public string Surname { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.TeamCategory")]
        public int TeamCategoryId { get; set; }
        public virtual TeamCategoryModel TeamCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOnHome")]
        public bool ShowOnHome { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class TeamListModel : BasePagedListModel<TeamModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class TeamSearchModel : BaseSearchModel
    {
        #region Ctor

        public TeamSearchModel()
        {
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Name")]
        public string Name { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Surname")]
        public string Surname { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.TeamCategory")]
        public int? TeamCategoryId { get; set; }
        public virtual TeamCategoryModel TeamCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOnHome")]
        public bool ShowOnHome { get; set; }
        #endregion
    }
}
