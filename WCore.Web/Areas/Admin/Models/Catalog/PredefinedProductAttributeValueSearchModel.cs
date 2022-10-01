using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a predefined product attribute value search model
    /// </summary>
    public partial class PredefinedProductAttributeValueSearchModel : BaseSearchModel
    {
        #region Properties

        public int ProductAttributeId { get; set; }

        #endregion
    }
}