using WCore.Core.Domain.Stores;
using WCore.Services.Caching;
using WCore.Services.Localization;

namespace WCore.Services.Stores.Caching
{
    /// <summary>
    /// Represents a store cache event consumer
    /// </summary>
    public partial class StoreCacheEventConsumer : CacheEventConsumer<Store>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Store entity)
        {
            Remove(WCoreStoreDefaults.StoresAllCacheKey);
            //RemoveByPrefix(WCoreOrdersDefaults.ShoppingCartPrefixCacheKey);

            var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreLocalizationDefaults.LanguagesByStoreIdPrefixCacheKey, entity);

            RemoveByPrefix(prefix);
        }
    }
}
