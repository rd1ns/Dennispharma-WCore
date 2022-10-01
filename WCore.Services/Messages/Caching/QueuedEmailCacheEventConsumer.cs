using WCore.Core.Domain.Messages;
using WCore.Services.Caching;

namespace WCore.Services.Messages.Caching
{
    /// <summary>
    /// Represents an queued email cache event consumer
    /// </summary>
    public partial class QueuedEmailCacheEventConsumer : CacheEventConsumer<QueuedEmail>
    {
    }
}
