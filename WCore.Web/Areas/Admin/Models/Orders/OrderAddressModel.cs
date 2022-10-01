using WCore.Web.Areas.Admin.Models.Common;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Orders
{
    public partial class OrderAddressModel : BaseWCoreModel
    {
        #region Ctor

        public OrderAddressModel()
        {
            Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int OrderId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}