using WCore.Core.Domain.Affiliates;
using WCore.Services.Caching;

namespace WCore.Services.Affiliates.Caching
{
    /// <summary>
    /// Represents an affiliate cache event consumer
    /// </summary>
    public partial class AffiliateCacheEventConsumer : CacheEventConsumer<Affiliate>
    {
    }
}
