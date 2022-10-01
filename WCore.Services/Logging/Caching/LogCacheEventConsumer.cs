using WCore.Core.Domain.Logging;
using WCore.Services.Caching;

namespace WCore.Services.Logging.Caching
{
    /// <summary>
    /// Represents a log cache event consumer
    /// </summary>
    public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
    {
    }
}
