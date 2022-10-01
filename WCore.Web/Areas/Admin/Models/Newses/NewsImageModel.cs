using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Newses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class NewsImageModel : BaseWCoreEntityModel, ILocalizedModel<NewsImageLocalizedModel>
    {
        #region Ctor
        public NewsImageModel()
        {
            Locales = new List<NewsImageLocalizedModel>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Slogan")]
        public string Slogan { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Small")]
        public string Small { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Medium")]
        public string Medium { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Big")]
        public string Big { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Original")]
        public string Original { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.News")]
        public int NewsId { get; set; }
        public virtual NewsModel News { get; set; }

        public IList<NewsImageLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class NewsImageLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Slogan")]
        public string Slogan { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents a NewsImage list model
    /// </summary>
    public partial class NewsImageListModel : BasePagedListModel<NewsImageModel>
    {
    }

    /// <summary>
    /// Represents a NewsImage search model
    /// </summary>
    public partial class NewsImageSearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsImageSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.News")]
        public int NewsId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.News")]
        public NewsModel News { get; set; }
        #endregion
    }
}
