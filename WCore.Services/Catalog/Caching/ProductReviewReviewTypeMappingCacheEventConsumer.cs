using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product review review type cache event consumer
    /// </summary>
    public partial class ProductReviewReviewTypeMappingCacheEventConsumer : CacheEventConsumer<ProductReviewReviewType>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ProductReviewReviewType entity)
        {
            Remove(WCoreCatalogDefaults.ReviewTypeAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(WCoreCatalogDefaults.ProductReviewReviewTypeMappingAllCacheKey, entity.ProductReviewId);
            Remove(cacheKey);
        }
    }
}
