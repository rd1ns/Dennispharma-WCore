using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a gift card usage history cache event consumer
    /// </summary>
    public partial class GiftCardUsageHistoryCacheEventConsumer : CacheEventConsumer<GiftCardUsageHistory>
    {
    }
}
