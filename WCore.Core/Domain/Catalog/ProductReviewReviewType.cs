using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product review and review type mapping
    /// </summary>
    public partial class ProductReviewReviewType : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the product review identifier
        /// </summary>
        public int ProductReviewId { get; set; }

        /// <summary>
        /// Gets or sets the review type identifier
        /// </summary>
        public int ReviewTypeId { get; set; }

        /// <summary>
        /// Gets or sets the rating
        /// </summary>
        public int Rating { get; set; }
    }
}
