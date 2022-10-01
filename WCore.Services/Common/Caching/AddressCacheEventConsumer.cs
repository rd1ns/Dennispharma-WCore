using WCore.Core.Domain.Common;
using WCore.Services.Caching;
using WCore.Services.Users;

namespace WCore.Services.Common.Caching
{
    /// <summary>
    /// Represents a address cache event consumer
    /// </summary>
    public partial class AddressCacheEventConsumer : CacheEventConsumer<Address>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Address entity)
        {
            RemoveByPrefix(WCoreUserServicesDefaults.UserAddressesPrefixCacheKey);
        }
    }
}
