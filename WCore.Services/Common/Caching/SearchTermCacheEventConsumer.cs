using WCore.Core.Domain.Common;
using WCore.Services.Caching;

namespace WCore.Services.Common.Caching
{
    /// <summary>
    /// Represents a search term cache event consumer
    /// </summary>
    public partial class SearchTermCacheEventConsumer : CacheEventConsumer<SearchTerm>
    {
    }
}
