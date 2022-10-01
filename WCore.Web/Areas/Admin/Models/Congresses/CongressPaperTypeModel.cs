using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Congresses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class CongressPaperTypeModel : BaseWCoreEntityModel, ILocalizedModel<CongressPaperTypeLocalizedModel>
    {
        #region Ctor
        public CongressPaperTypeModel()
        {
            Locales = new List<CongressPaperTypeLocalizedModel>();
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public int CongressId { get; set; }
        public CongressModel Congress { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        public IList<CongressPaperTypeLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class CongressPaperTypeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class CongressPaperTypeListModel : BasePagedListModel<CongressPaperTypeModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class CongressPaperTypeSearchModel : BaseSearchModel
    {
        #region Ctor

        public CongressPaperTypeSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public int? CongressId { get; set; }

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
