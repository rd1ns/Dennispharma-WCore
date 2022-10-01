using WCore.Core.Domain.Pages;
using WCore.Services.Caching;

namespace WCore.Services.Pages.Caching
{
    /// <summary>
    /// Represents an affiliate cache event consumer
    /// </summary>
    public partial class PageCacheEventConsumer : CacheEventConsumer<Page>
    {
    }
}
