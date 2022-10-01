using WCore.Web.Areas.Admin.Models.Common;
using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user address model
    /// </summary>
    public partial class UserAddressModel : BaseWCoreModel
    {
        #region Ctor

        public UserAddressModel()
        {
            Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}