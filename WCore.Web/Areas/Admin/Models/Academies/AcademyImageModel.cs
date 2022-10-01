using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Academies
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class AcademyImageModel : BaseWCoreEntityModel, ILocalizedModel<AcademyImageLocalizedModel>
    {
        #region Ctor
        public AcademyImageModel()
        {
            Locales = new List<AcademyImageLocalizedModel>();
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

        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public int AcademyId { get; set; }
        public virtual AcademyModel Academy { get; set; }

        public IList<AcademyImageLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a AcademyImage list model
    /// </summary>
    public partial class AcademyImageListModel : BasePagedListModel<AcademyImageModel>
    {
    }
    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class AcademyImageLocalizedModel : ILocalizedLocaleModel
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
    /// Represents a AcademyImage search model
    /// </summary>
    public partial class AcademyImageSearchModel : BaseSearchModel
    {
        #region Ctor

        public AcademyImageSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public int AcademyId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public AcademyModel Academy { get; set; }
        #endregion
    }
}
