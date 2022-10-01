using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product review cache event consumer
    /// </summary>
    public partial class ProductReviewCacheEventConsumer : CacheEventConsumer<ProductReview>
    {
    }
}
