using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Galleries
{
    public partial class GalleryImageModel : BaseWCoreEntityModel, ILocalizedModel<GalleryImageLocalizedModel>
    {
        #region Ctor
        public GalleryImageModel()
        {
            Locales = new List<GalleryImageLocalizedModel>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Slogan")]
        public string Slogan { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Link")]
        public string Link { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LinkText")]
        public string LinkText { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Small")]
        public string Small { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Medium")]
        public string Medium { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Big")]
        public string Big { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Original")]
        public string Original { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.VideoLink")]
        public string VideoLink { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsVideo")]
        public bool IsVideo { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Gallery")]
        public int GalleryId { get; set; }
        public virtual GalleryModel Gallery { get; set; }

        public IList<GalleryImageLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class GalleryImageLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Slogan")]
        public string Slogan { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public string Description { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Link")]
        public string Link { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LinkText")]
        public string LinkText { get; set; }
    }

    /// <summary>
    /// Represents a GalleryImage list model
    /// </summary>
    public partial class GalleryImageListModel : BasePagedListModel<GalleryImageModel>
    {
    }
    /// <summary>
    /// Represents a GalleryImage search model
    /// </summary>
    public partial class GalleryImageSearchModel : BaseSearchModel
    {
        #region Ctor

        public GalleryImageSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Gallery")]
        public int GalleryId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Gallery")]
        public GalleryModel Gallery { get; set; }
        #endregion
    }
}
