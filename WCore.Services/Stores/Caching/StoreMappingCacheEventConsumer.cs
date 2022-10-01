using WCore.Core.Domain.Stores;
using WCore.Services.Caching;

namespace WCore.Services.Stores.Caching
{
    /// <summary>
    /// Represents a store mapping cache event consumer
    /// </summary>
    public partial class StoreMappingCacheEventConsumer : CacheEventConsumer<StoreMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(StoreMapping entity)
        {
            var entityId = entity.EntityId;
            var entityName = entity.EntityName;

            var key = _cacheKeyService.PrepareKey(WCoreStoreDefaults.StoreMappingsByEntityIdNameCacheKey, entityId, entityName);

            Remove(key);

            key = _cacheKeyService.PrepareKey(WCoreStoreDefaults.StoreMappingIdsByEntityIdNameCacheKey, entityId, entityName);
            
            Remove(key);
        }
    }
}
