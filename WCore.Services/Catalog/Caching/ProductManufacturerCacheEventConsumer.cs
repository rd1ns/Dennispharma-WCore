using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product manufacturer cache event consumer
    /// </summary>
    public partial class ProductManufacturerCacheEventConsumer : CacheEventConsumer<ProductManufacturer>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ProductManufacturer entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductManufacturersByProductPrefixCacheKey, entity.ProductId);
            RemoveByPrefix(prefix);
            
            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductPricePrefixCacheKey, entity.ProductId);
            RemoveByPrefix(prefix);
        }
    }
}
