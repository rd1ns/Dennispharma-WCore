using WCore.Core.Domain.Common;
using WCore.Services.Caching;

namespace WCore.Services.Common.Caching
{
    /// <summary>
    /// Represents a generic attribute cache event consumer
    /// </summary>
    public partial class GenericAttributeCacheEventConsumer : CacheEventConsumer<GenericAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(GenericAttribute entity)
        {
            var cacheKey = _cacheKeyService.PrepareKey(WCoreCommonDefaults.GenericAttributeCacheKey, entity.EntityId, entity.KeyGroup);
            Remove(cacheKey);
        }
    }
}
