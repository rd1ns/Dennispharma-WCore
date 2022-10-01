using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents an order item cache event consumer
    /// </summary>
    public partial class OrderItemCacheEventConsumer : CacheEventConsumer<OrderItem>
    {
    }
}
