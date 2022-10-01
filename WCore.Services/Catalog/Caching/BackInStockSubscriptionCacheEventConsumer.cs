using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;

namespace WCore.Services.Catalog.Caching
{
    /// <summary>
    /// Represents a back in stock subscription cache event consumer
    /// </summary>
    public partial class BackInStockSubscriptionCacheEventConsumer : CacheEventConsumer<BackInStockSubscription>
    {
    }
}
