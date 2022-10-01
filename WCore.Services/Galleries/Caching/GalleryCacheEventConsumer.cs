using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Pages;
using WCore.Services.Caching;

namespace WCore.Services.Galleries.Caching
{
    /// <summary>
    /// Represents an affiliate cache event consumer
    /// </summary>
    public partial class GalleryCacheEventConsumer : CacheEventConsumer<Gallery>
    {
    }
    public partial class GalleryImageCacheEventConsumer : CacheEventConsumer<GalleryImage>
    {
    }
}
