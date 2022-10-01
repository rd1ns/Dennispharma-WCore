using WCore.Core.Domain.Common;
using WCore.Services.Caching;

namespace WCore.Services.Common.Caching
{
    /// <summary>
    /// Represents a address attribute value cache event consumer
    /// </summary>
    public partial class AddressAttributeValueCacheEventConsumer : CacheEventConsumer<AddressAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(AddressAttributeValue entity)
        {
            //Remove(WCoreCommonDefaults.AddressAttributesAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(WCoreCommonDefaults.AddressAttributeValuesAllCacheKey, entity.AddressAttributeId);
            Remove(cacheKey);
        }
    }
}
