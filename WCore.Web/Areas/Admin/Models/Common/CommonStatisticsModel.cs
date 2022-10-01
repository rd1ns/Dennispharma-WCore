using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Common
{
    public partial class CommonStatisticsModel : BaseWCoreModel
    {
        public int NumberOfOrders { get; set; }

        public int NumberOfUsers { get; set; }

        public int NumberOfPendingReturnRequests { get; set; }

        public int NumberOfLowStockProducts { get; set; }
    }
}