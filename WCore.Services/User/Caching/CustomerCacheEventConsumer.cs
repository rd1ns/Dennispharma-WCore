using WCore.Core.Domain.Users;
using WCore.Services.Caching;
using WCore.Services.Events;
using WCore.Services.Orders;

namespace WCore.Services.Users.Caching
{
    /// <summary>
    /// Represents a user cache event consumer
    /// </summary>
    public partial class UserCacheEventConsumer : CacheEventConsumer<User>, IConsumer<UserPasswordChangedEvent>
    {
        #region Methods

        /// <summary>
        /// Handle password changed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(UserPasswordChangedEvent eventMessage)
        {
            Remove(_cacheKeyService.PrepareKey(WCoreUserServicesDefaults.UserPasswordLifetimeCacheKey, eventMessage.Password.UserId));
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(User entity)
        {
            RemoveByPrefix(WCoreUserServicesDefaults.UserUserRolesPrefixCacheKey);
            RemoveByPrefix(WCoreUserServicesDefaults.UserAddressesPrefixCacheKey);
            RemoveByPrefix(WCoreOrderDefaults.ShoppingCartPrefixCacheKey);
        }

        #endregion
    }
}