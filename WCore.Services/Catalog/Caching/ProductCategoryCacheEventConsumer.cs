using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product category cache event consumer
    /// </summary>
    public partial class ProductCategoryCacheEventConsumer : CacheEventConsumer<ProductCategory>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ProductCategory entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductCategoriesByProductPrefixCacheKey, entity.ProductId);
            RemoveByPrefix(prefix);

            RemoveByPrefix(WCoreCatalogDefaults.CategoryNumberOfProductsPrefixCacheKey);
            
            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductPricePrefixCacheKey, entity.ProductId);
            RemoveByPrefix(prefix);
        }
    }
}
