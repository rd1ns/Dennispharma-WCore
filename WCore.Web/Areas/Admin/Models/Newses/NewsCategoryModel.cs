using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Newses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class NewsCategoryModel : BaseWCoreEntityModel, ILocalizedModel<NewsCategoryLocalizedModel>
    {
        #region Ctor
        public NewsCategoryModel()
        {
            Locales = new List<NewsCategoryLocalizedModel>();
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
        public string MetaKeywords { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }

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

        public IList<NewsCategoryLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class NewsCategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class NewsCategoryListModel : BasePagedListModel<NewsCategoryModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class NewsCategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsCategorySearchModel()
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
        #endregion
    }
}
