using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Discounts;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a manufacturer cache event consumer
    /// </summary>
    public partial class ManufacturerCacheEventConsumer : CacheEventConsumer<Manufacturer>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Manufacturer entity)
        {
            RemoveByPrefix(WCoreDiscountDefaults.DiscountManufacturerIdsPrefixCacheKey);
        }
    }
}
