using WCore.Core.Domain.Orders;
using WCore.Services.Caching;

namespace WCore.Services.Orders.Caching
{
    /// <summary>
    /// Represents a recurring payment history cache event consumer
    /// </summary>
    public partial class RecurringPaymentHistoryCacheEventConsumer : CacheEventConsumer<RecurringPaymentHistory>
    {
    }
}
