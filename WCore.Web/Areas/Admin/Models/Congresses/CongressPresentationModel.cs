using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Congresses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class CongressPresentationModel : BaseWCoreEntityModel, ILocalizedModel<CongressPresentationLocalizedModel>
    {
        #region Ctor
        public CongressPresentationModel()
        {
            Locales = new List<CongressPresentationLocalizedModel>();
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Code")]
        public string Code { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.OwnersName")]
        public string OwnersName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.OwnersSurname")]
        public string OwnersSurname { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.FilePath")]
        public string FilePath { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Congress")]
        public int CongressId { get; set; }
        public CongressModel Congress { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CongressPresentationType")]
        public int CongressPresentationTypeId { get; set; }
        public CongressPresentationTypeModel CongressPresentationType { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        public IList<CongressPresentationLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class CongressPresentationLocalizedModel : ILocalizedLocaleModel
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
    public partial class CongressPresentationListModel : BasePagedListModel<CongressPresentationModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class CongressPresentationSearchModel : BaseSearchModel
    {
        #region Ctor

        public CongressPresentationSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Code")]
        public string Code { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.OwnersName")]
        public string OwnersName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.OwnersSurname")]
        public string OwnersSurname { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.CongressPresentationType")]
        public int? CongressPresentationTypeId { get; set; }

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
