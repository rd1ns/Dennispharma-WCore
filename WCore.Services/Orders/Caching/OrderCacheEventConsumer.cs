using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a order cache event consumer
    /// </summary>
    public partial class OrderCacheEventConsumer : CacheEventConsumer<Order>
    {
    }
}