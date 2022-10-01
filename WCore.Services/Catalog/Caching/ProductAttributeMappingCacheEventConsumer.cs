using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product attribute mapping cache event consumer
    /// </summary>
    public partial class ProductProductAttributeCacheEventConsumer : CacheEventConsumer<ProductProductAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ProductProductAttribute entity)
        {
            var cacheKey = _cacheKeyService.PrepareKey(WCoreCatalogDefaults.ProductProductAttributesAllCacheKey, entity.ProductId);
            Remove(cacheKey);

            cacheKey = _cacheKeyService.PrepareKey(WCoreCatalogDefaults.ProductAttributeValuesAllCacheKey, entity);
            Remove(cacheKey);

            cacheKey = _cacheKeyService.PrepareKey(WCoreCatalogDefaults.ProductAttributeCombinationsAllCacheKey, entity.ProductId);
            Remove(cacheKey);
        }
    }
}
