using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a product-product tag mapping  cache event consumer
    /// </summary>
    public partial class ProductProductTagMappingCacheEventConsumer : CacheEventConsumer<ProductProductTag>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ProductProductTag entity)
        {
            Remove(_cacheKeyService.PrepareKey(WCoreCatalogDefaults.ProductTagAllByProductIdCacheKey, entity.ProductId));
        }
    }
}