using WCore.Core.Domain.Users;
using WCore.Services.Caching;

namespace WCore.Services.Users.Caching
{
    /// <summary>
    /// Represents a user attribute cache event consumer
    /// </summary>
    public partial class UserAttributeCacheEventConsumer : CacheEventConsumer<UserAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(UserAttribute entity)
        {
            Remove(WCoreUserServicesDefaults.UserAttributesAllCacheKey);
            Remove(_cacheKeyService.PrepareKey(WCoreUserServicesDefaults.UserAttributeValuesAllCacheKey, entity));
        }
    }
}
