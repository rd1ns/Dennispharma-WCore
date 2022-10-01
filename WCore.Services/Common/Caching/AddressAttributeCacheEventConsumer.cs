using WCore.Core.Domain.Common;
using WCore.Services.Caching;

namespace WCore.Services.Common.Caching
{
    /// <summary>
    /// Represents a address attribute cache event consumer
    /// </summary>
    public partial class AddressAttributeCacheEventConsumer : CacheEventConsumer<AddressAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(AddressAttribute entity)
        {
            //Remove(WCoreCommonDefaults.AddressAttributesAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(WCoreCommonDefaults.AddressAttributeValuesAllCacheKey, entity);
            Remove(cacheKey);
        }
    }
}
