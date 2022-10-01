using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WCore.Core.Domain.Academies;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Academies
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class AcademyVideoModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyVideoModel()
        {
            AcademyVideoResources = new List<SelectListItem>();
        }
        #endregion
        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Path")]
        public string Path { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AcademyVideoResource")]
        public AcademyVideoResource AcademyVideoResource { get; set; }
        public string AcademyVideoResourceName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Small")]
        public string Small { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Medium")]
        public string Medium { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Big")]
        public string Big { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Original")]
        public string Original { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public int AcademyId { get; set; }
        public virtual AcademyModel Academy { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        public List<SelectListItem> AcademyVideoResources { get;set;}
        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class AcademyVideoLocalizedModel : ILocalizedLocaleModel
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
    /// Represents a AcademyVideo list model
    /// </summary>
    public partial class AcademyVideoListModel : BasePagedListModel<AcademyVideoModel>
    {
    }

    /// <summary>
    /// Represents a AcademyVideo search model
    /// </summary>
    public partial class AcademyVideoSearchModel : BaseSearchModel
    {
        #region Ctor

        public AcademyVideoSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public int AcademyId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public AcademyModel Academy { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Description")]
        public AcademyVideoResource? AcademyVideoResource { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        #endregion
    }
}
