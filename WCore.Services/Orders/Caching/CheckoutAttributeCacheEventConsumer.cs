using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a checkout attribute cache event consumer
    /// </summary>
    public partial class CheckoutAttributeCacheEventConsumer : CacheEventConsumer<CheckoutAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CheckoutAttribute entity)
        {
            RemoveByPrefix(WCoreOrderDefaults.CheckoutAttributesAllPrefixCacheKey);
            var cacheKey = _cacheKeyService.PrepareKey(WCoreOrderDefaults.CheckoutAttributeValuesAllCacheKey, entity);
            Remove(cacheKey);
        }
    }
}
