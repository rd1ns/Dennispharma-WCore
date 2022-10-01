using WCore.Core.Domain.Users;
using WCore.Services.Caching;

namespace WCore.Services.Users.Caching
{
    /// <summary>
    /// Represents a user address mapping cache event consumer
    /// </summary>
    public partial class UserAddressMappingCacheEventConsumer : CacheEventConsumer<UserAddress>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(UserAddress entity)
        {
            RemoveByPrefix(WCoreUserServicesDefaults.UserAddressesPrefixCacheKey);
        }
    }
}