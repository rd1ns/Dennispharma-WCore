using WCore.Core.Domain.Discounts;
using WCore.Services.Caching;

namespace WCore.Services.Discounts.Caching
{
    /// <summary>
    /// Represents a discount usage history cache event consumer
    /// </summary>
    public partial class DiscountUsageHistoryCacheEventConsumer : CacheEventConsumer<DiscountUsageHistory>
    {
    }
}
