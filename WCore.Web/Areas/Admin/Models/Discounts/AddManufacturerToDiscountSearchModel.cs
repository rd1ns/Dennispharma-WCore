using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a manufacturer search model to add to the discount
    /// </summary>
    public partial class AddManufacturerToDiscountSearchModel : BaseSearchModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
        public string SearchManufacturerName { get; set; }

        #endregion
    }
}