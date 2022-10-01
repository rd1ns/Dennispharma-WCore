using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Orders;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product cache event consumer
    /// </summary>
    public partial class ProductCacheEventConsumer : CacheEventConsumer<Product>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Product entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductManufacturersByProductPrefixCacheKey, entity);
            RemoveByPrefix(prefix);

            Remove(WCoreCatalogDefaults.ProductsAllDisplayedOnHomepageCacheKey);
            RemoveByPrefix(WCoreCatalogDefaults.ProductsByIdsPrefixCacheKey);

            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.ProductPricePrefixCacheKey, entity);
            RemoveByPrefix(prefix);

            RemoveByPrefix(WCoreOrderDefaults.ShoppingCartPrefixCacheKey);
        }
    }
}
