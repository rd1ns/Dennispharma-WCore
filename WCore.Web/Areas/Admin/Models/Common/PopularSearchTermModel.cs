using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a popular search term model
    /// </summary>
    public partial class PopularSearchTermModel : BaseWCoreModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.SearchTermReport.Keyword")]
        public string Keyword { get; set; }

        [WCoreResourceDisplayName("Admin.SearchTermReport.Count")]
        public int Count { get; set; }

        #endregion
    }
}
