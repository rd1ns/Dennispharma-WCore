using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Congresses
{
    /// <summary>
    /// Represents entity model
    /// </summary>
    public partial class CongressModel : BaseWCoreEntityModel, ILocalizedModel<CongressLocalizedModel>
    {
        #region Ctor
        public CongressModel()
        {
            Locales = new List<CongressLocalizedModel>();
        }
        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MiniTitle")]
        public string MiniTitle { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AwardWinningWorks")]
        public string AwardWinningWorks { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Banner")]
        public string Banner { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.StartDate")]
        public DateTime StartDate { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.EndDate")]
        public DateTime EndDate { get; set; }
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
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }

        public IList<CongressLocalizedModel> Locales { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents localized entity model
    /// </summary>
    public partial class CongressLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string MiniTitle { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.AwardWinningWorks")]
        public string AwardWinningWorks { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }

    /// <summary>
    /// Represents a list model
    /// </summary>
    public partial class CongressListModel : BasePagedListModel<CongressModel>
    {
    }

    /// <summary>
    /// Represents a search model
    /// </summary>
    public partial class CongressSearchModel : BaseSearchModel
    {
        #region Ctor

        public CongressSearchModel()
        {
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

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
        #endregion
    }
}
