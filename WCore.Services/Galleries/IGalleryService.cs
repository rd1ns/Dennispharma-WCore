using WCore.Core;
using WCore.Core.Domain.Galleries;

namespace WCore.Services.Galleries
{
    public interface IGalleryService : IRepository<Gallery>
    {
        IPagedList<Gallery> GetAllByFilters(GalleryType? GalleryType = null, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null, int skip = 0, int take = int.MaxValue);
        Gallery GetByGalleryType(GalleryType GalleryType, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null);
    }
    public interface IGalleryImageService : IRepository<GalleryImage>
    {
        IPagedList<GalleryImage> GetAllByFilters(int galleryId, int skip = 0, int take = int.MaxValue);
    }
}
