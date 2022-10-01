using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Academies
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class AcademyCategoryModel : BaseWCoreEntityModel, ILocalizedModel<AcademyCategoryLocalizedModel>
    {
        #region Ctor
        public AcademyCategoryModel()
        {
            Locales = new List<AcademyCategoryLocalizedModel>();
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Banner")]
        public string Banner { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AcademyCategory")]
        public int ParentId { get; set; }
        public AcademyCategoryModel Parent { get; set; }

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

        public IList<AcademyCategoryLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class AcademyCategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class AcademyCategoryListModel : BasePagedListModel<AcademyCategoryModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class AcademyCategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public AcademyCategorySearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AcademyCategory")]
        public int? ParentId { get; set; }
        public AcademyCategoryModel Parent { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        #endregion
    }
}
