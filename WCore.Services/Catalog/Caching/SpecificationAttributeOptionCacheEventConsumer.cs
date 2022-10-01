using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a specification attribute option cache event consumer
    /// </summary>
    public partial class SpecificationAttributeOptionCacheEventConsumer : CacheEventConsumer<SpecificationAttributeOption>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(SpecificationAttributeOption entity)
        {
            Remove(WCoreCatalogDefaults.SpecAttributesWithOptionsCacheKey);
            Remove(_cacheKeyService.PrepareKey(WCoreCatalogDefaults.SpecAttributesOptionsCacheKey, entity.SpecificationAttributeId));

            RemoveByPrefix(WCoreCatalogDefaults.ProductSpecificationAttributeAllByProductIdsPrefixCacheKey);
        }
    }
}
