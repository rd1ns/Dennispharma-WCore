using WCore.Core.Domain.Vendors;
using WCore.Services.Caching;

namespace WCore.Services.Vendors.Caching
{
    /// <summary>
    /// Represents a vendor attribute cache event consumer
    /// </summary>
    public partial class VendorAttributeCacheEventConsumer : CacheEventConsumer<VendorAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(VendorAttribute entity)
        {
            base.Remove(WCoreVendorDefaults.VendorAttributesAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(WCoreVendorDefaults.VendorAttributeValuesAllCacheKey, entity);

            Remove(cacheKey);
        }
    }
}
