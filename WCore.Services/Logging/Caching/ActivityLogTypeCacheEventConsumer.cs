using WCore.Core.Domain.Logging;
using WCore.Services.Caching;

namespace WCore.Services.Logging.Caching
{
    /// <summary>
    /// Represents a activity log type cache event consumer
    /// </summary>
    public partial class ActivityLogTypeCacheEventConsumer : CacheEventConsumer<ActivityLogType>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ActivityLogType entity)
        {
            Remove(WCoreLoggingDefaults.ActivityTypeAllCacheKey);
        }
    }
}
