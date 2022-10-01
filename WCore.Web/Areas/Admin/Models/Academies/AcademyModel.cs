using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Academies
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class AcademyModel : BaseWCoreEntityModel, ILocalizedModel<AcademyLocalizedModel>
    {
        #region Ctor
        public AcademyModel()
        {
            Locales = new List<AcademyLocalizedModel>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Banner")]
        public string Banner { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AcademyCategory")]
        public int AcademyCategoryId { get; set; }
        public virtual AcademyCategoryModel AcademyCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public bool IsArchived { get; set; }
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

        public IList<AcademyLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class AcademyLocalizedModel : ILocalizedLocaleModel
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
    public partial class AcademyListModel : BasePagedListModel<AcademyModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class AcademySearchModel : BaseSearchModel
    {
        #region Ctor

        public AcademySearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.AcademyCategory")]
        public int? AcademyCategoryId { get; set; }
        public virtual AcademyCategoryModel AcademyCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsArchived")]
        public bool? IsArchived { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        #endregion
    }
}
