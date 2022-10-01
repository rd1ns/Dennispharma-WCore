using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Teams
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class TeamCategoryModel : BaseWCoreEntityModel, ILocalizedModel<TeamCategoryLocalizedModel>
    {
        #region Ctor
        public TeamCategoryModel()
        {
            Locales = new List<TeamCategoryLocalizedModel>();
            Parents = new List<SelectListItem>();
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Parent")]
        public int ParentId { get; set; }
        public TeamCategoryModel Parent { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }

        public IList<TeamCategoryLocalizedModel> Locales { get; set; }
        public List<SelectListItem> Parents { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class TeamCategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class TeamCategoryListModel : BasePagedListModel<TeamCategoryModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class TeamCategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public TeamCategorySearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Parent")]
        public int? ParentId { get; set; }
        #endregion
    }
}
