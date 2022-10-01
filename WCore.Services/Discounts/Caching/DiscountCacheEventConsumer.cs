using WCore.Core.Domain.Discounts;
using WCore.Services.Caching;

namespace WCore.Services.Discounts.Caching
{
    /// <summary>
    /// Represents a discount cache event consumer
    /// </summary>
    public partial class DiscountCacheEventConsumer : CacheEventConsumer<Discount>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Discount entity)
        {
            RemoveByPrefix(WCoreDiscountDefaults.DiscountAllPrefixCacheKey);
            var cacheKey = _cacheKeyService.PrepareKey(WCoreDiscountDefaults.DiscountRequirementModelCacheKey, entity);
            Remove(cacheKey);

            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreDiscountDefaults.DiscountCategoryIdsByDiscountPrefixCacheKey, entity);
            RemoveByPrefix(prefix);

            prefix = _cacheKeyService.PrepareKeyPrefix(WCoreDiscountDefaults.DiscountManufacturerIdsByDiscountPrefixCacheKey, entity);
            RemoveByPrefix(prefix);
        }
    }
}
