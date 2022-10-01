using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Newses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class NewsModel : BaseWCoreEntityModel, ILocalizedModel<NewsLocalizedModel>
    {
        #region Ctor
        public NewsModel()
        {
            Locales = new List<NewsLocalizedModel>();
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
        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.NewsCategory")]
        public int NewsCategoryId { get; set; }
        public virtual NewsCategoryModel NewsCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.StartDate")]
        public DateTime StartDate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.EndDate")]
        public DateTime EndDate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
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
        [WCoreResourceDisplayName("Admin.Configuration.ShowOnHome")]
        public bool ShowOnHome { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }

        public IList<NewsLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class NewsLocalizedModel : ILocalizedLocaleModel
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
    public partial class NewsListModel : BasePagedListModel<NewsModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class NewsSearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.NewsCategory")]
        public int? NewsCategoryId { get; set; }
        public virtual NewsCategoryModel NewsCategory { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.StartDate")]
        public DateTime? StartDate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.EndDate")]
        public DateTime? EndDate { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsArchived")]
        public bool? IsArchived { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOnHome")]
        public bool? ShowOnHome { get; set; }
        #endregion
    }
}
