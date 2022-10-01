using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Academies
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class AcademyFileModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyFileModel()
        {
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Path")]
        public string Path { get; set; }

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

        #endregion
    }

    /// <summary>
    /// Represents a AcademyFile list model
    /// </summary>
    public partial class AcademyFileListModel : BasePagedListModel<AcademyFileModel>
    {
    }

    /// <summary>
    /// Represents a AcademyFile search model
    /// </summary>
    public partial class AcademyFileSearchModel : BaseSearchModel
    {
        #region Ctor

        public AcademyFileSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public int AcademyId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Academy")]
        public AcademyModel Academy { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        #endregion
    }
}
