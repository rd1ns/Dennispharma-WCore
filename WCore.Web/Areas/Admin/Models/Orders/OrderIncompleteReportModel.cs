using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents an incomplete order report model
    /// </summary>
    public partial class OrderIncompleteReportModel : BaseWCoreModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.SalesReport.Incomplete.Item")]
        public string Item { get; set; }

        [WCoreResourceDisplayName("Admin.SalesReport.Incomplete.Total")]
        public string Total { get; set; }

        [WCoreResourceDisplayName("Admin.SalesReport.Incomplete.Count")]
        public int Count { get; set; }

        [WCoreResourceDisplayName("Admin.SalesReport.Incomplete.View")]
        public string ViewLink { get; set; }

        #endregion
    }
}