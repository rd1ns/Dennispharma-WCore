using WCore.Core.Domain.Users;
using WCore.Services.Caching;

namespace WCore.Services.Users.Caching
{
    /// <summary>
    /// Represents a user attribute value cache event consumer
    /// </summary>
    public partial class UserAttributeValueCacheEventConsumer : CacheEventConsumer<UserAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(UserAttributeValue entity)
        {
            Remove(WCoreUserServicesDefaults.UserAttributesAllCacheKey);
            Remove(_cacheKeyService.PrepareKey(WCoreUserServicesDefaults.UserAttributeValuesAllCacheKey, entity.UserAttributeId));
        }
    }
}