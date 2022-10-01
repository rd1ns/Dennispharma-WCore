using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Galleries;
using WCore.Services.Users;

namespace WCore.Services.Galleries
{
    public class GalleryService : Repository<Gallery>, IGalleryService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;
        public GalleryService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext,
            IUserAgencyAuthorizationService userAgencyAuthorizationService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._workContext = workContext;
        }

        public IPagedList<Gallery> GetAllByFilters(GalleryType? GalleryType = null, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null, int skip = 0, int take = int.MaxValue)
        {
            IQueryable<Gallery> recordsFiltered = context.Set<Gallery>();

            if (GalleryType.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.GalleryType == GalleryType);

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.ShowOn == ShowOn);


            int recordsFilteredCount = recordsFiltered.Count();

            var data = recordsFiltered.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(skip).Take(take).ToList();

            return new PagedList<Gallery>(data, skip, take, recordsFilteredCount);
        }
        public Gallery GetByGalleryType(GalleryType GalleryType, bool? IsActive = null, bool? Deleted = null, bool? ShowOn = null)
        {
            IQueryable<Gallery> recordsFiltered = context.Set<Gallery>();

            recordsFiltered = recordsFiltered.Where(a => a.GalleryType == GalleryType);

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.ShowOn == ShowOn);

            var data = recordsFiltered.OrderByDescending(o => o.IsActive).FirstOrDefault(o => o.GalleryType == GalleryType);

            return data;
        }
    }
    public class GalleryImageService : Repository<GalleryImage>, IGalleryImageService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IWorkContext _workContext;
        public GalleryImageService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            IWorkContext workContext,
            IUserAgencyAuthorizationService userAgencyAuthorizationService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._workContext = workContext;
        }

        public IPagedList<GalleryImage> GetAllByFilters(int galleryId, int skip = 0, int take = int.MaxValue)
        {
            IQueryable<GalleryImage> recordsFiltered = context.Set<GalleryImage>();

            recordsFiltered = recordsFiltered.Where(a => a.GalleryId == galleryId);

            int recordsFilteredCount = recordsFiltered.Count();

            var data = recordsFiltered.OrderBy(o => o.DisplayOrder).Skip(skip).Take(take).ToList();

            return new PagedList<GalleryImage>(data, skip, take, recordsFilteredCount);
        }
    }
}
