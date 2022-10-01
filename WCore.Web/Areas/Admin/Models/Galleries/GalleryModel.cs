using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Core.Domain.Galleries;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Galleries
{
    public partial class GalleryModel : BaseWCoreEntityModel, ILocalizedModel<GalleryLocalizedModel>
    {
        #region Ctor
        public GalleryModel()
        {
            Locales = new List<GalleryLocalizedModel>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.GalleryType")]
        public GalleryType GalleryType { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.GalleryType")]
        public string GalleryTypeName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }


        public IList<GalleryLocalizedModel> Locales { get; set; }
        public List<SelectListItem> GalleryTypes { get; set; }

        #endregion
    }

    public partial class GalleryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
    }

    /// <summary>
    /// Represents a gallery list model
    /// </summary>
    public partial class GalleryListModel : BasePagedListModel<GalleryModel>
    {
    }
    /// <summary>
    /// Represents a Gallery search model
    /// </summary>
    public partial class GallerySearchModel : BaseSearchModel
    {
        #region Ctor

        public GallerySearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.GalleryType")]
        public GalleryType? GalleryType { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.GalleryType")]
        public List<SelectListItem> GalleryTypes { get; set; }
        #endregion
    }
}
