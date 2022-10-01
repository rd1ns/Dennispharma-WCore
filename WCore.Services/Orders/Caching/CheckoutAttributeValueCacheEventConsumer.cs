using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a checkout attribute value cache event consumer
    /// </summary>
    public partial class CheckoutAttributeValueCacheEventConsumer : CacheEventConsumer<CheckoutAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CheckoutAttributeValue entity)
        {
            var cacheKey = _cacheKeyService.PrepareKey(WCoreOrderDefaults.CheckoutAttributeValuesAllCacheKey, entity.CheckoutAttributeId);
            Remove(cacheKey);
        }
    }
}
