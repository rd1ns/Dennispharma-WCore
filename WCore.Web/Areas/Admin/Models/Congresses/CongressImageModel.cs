using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Congresses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class CongressImageModel : BaseWCoreEntityModel, ILocalizedModel<CongressImageLocalizedModel>
    {
        #region Ctor
        public CongressImageModel()
        {
            Locales = new List<CongressImageLocalizedModel>();
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

        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public int CongressId { get; set; }
        public virtual CongressModel Congress { get; set; }

        public IList<CongressImageLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class CongressImageLocalizedModel : ILocalizedLocaleModel
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
    /// Represents a CongressImage list model
    /// </summary>
    public partial class CongressImageListModel : BasePagedListModel<CongressImageModel>
    {
    }

    /// <summary>
    /// Represents a CongressImage search model
    /// </summary>
    public partial class CongressImageSearchModel : BaseSearchModel
    {
        #region Ctor

        public CongressImageSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public int CongressId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public CongressModel Congress { get; set; }
        #endregion
    }
}
