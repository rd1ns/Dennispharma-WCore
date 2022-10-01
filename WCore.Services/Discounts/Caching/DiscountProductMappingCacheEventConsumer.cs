using WCore.Core.Domain.Discounts;
using WCore.Services.Caching;

namespace WCore.Services.Discounts.Caching
{
    /// <summary>
    /// Represents a discount-product mapping cache event consumer
    /// </summary>
    public partial class DiscountAppliedToProductCacheEventConsumer : CacheEventConsumer<DiscountAppliedToManufacturer>
    {
    }
}