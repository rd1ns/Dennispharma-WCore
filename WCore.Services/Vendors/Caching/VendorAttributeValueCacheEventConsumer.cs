using WCore.Core.Domain.Vendors;
using WCore.Services.Caching;

namespace WCore.Services.Vendors.Caching
{
    /// <summary>
    /// Represents a vendor attribute value cache event consumer
    /// </summary>
    public partial class VendorAttributeValueCacheEventConsumer : CacheEventConsumer<VendorAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(VendorAttributeValue entity)
        {
            var cacheKey = _cacheKeyService.PrepareKey(WCoreVendorDefaults.VendorAttributeValuesAllCacheKey, entity.VendorAttributeId);

            Remove(cacheKey);
        }
    }
}
