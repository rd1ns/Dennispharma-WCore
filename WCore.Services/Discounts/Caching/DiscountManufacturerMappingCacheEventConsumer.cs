using WCore.Core.Domain.Discounts;
using WCore.Services.Caching;

namespace WCore.Services.Discounts.Caching
{
    /// <summary>
    /// Represents a discount-manufacturer mapping cache event consumer
    /// </summary>
    public partial class DiscountManufacturerCacheEventConsumer : CacheEventConsumer<DiscountAppliedToManufacturer>
    {
    }
}