using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;

namespace WCore.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review and review type mapping model
    /// </summary>
    public class ProductReviewReviewTypeMappingModel : BaseWCoreEntityModel
    {
        #region Properties

        public int ProductReviewId { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Name")]
        public string Name { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Description")]
        public string Description { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.VisibleToAllUsers")]
        public bool VisibleToAllUsers { get; set; }

        [WCoreResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Rating")]
        public int Rating { get; set; }

        #endregion
    }
}
