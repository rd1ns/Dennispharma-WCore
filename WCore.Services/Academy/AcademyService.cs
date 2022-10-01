using System;
using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Academies;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;

namespace WCore.Services.Academies
{
    public class AcademyService : Repository<Academy>, IAcademyService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public AcademyService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<Academy> GetAllByFilters(int? AcademyCategoryId = null,
            string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<Academy> query = context.Set<Academy>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreAcademyDefaults.AllByFilters,
                AcademyCategoryId,
                Title,
                IsArchived,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take);

            if (AcademyCategoryId.HasValue)
                query = query.Where(a => a.AcademyCategoryId == AcademyCategoryId.Value);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (IsArchived.HasValue)
                query = query.Where(a => a.IsArchived == IsArchived);

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<Academy>(data, Skip, Take, queryCount);
        }
    }
    public class AcademyCategoryService : Repository<AcademyCategory>, IAcademyCategoryService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public AcademyCategoryService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<AcademyCategory> GetAllByFilters(int? ParentId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<AcademyCategory> query = context.Set<AcademyCategory>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreAcademyCategoryDefaults.AllByFilters,
                ParentId,
                Title,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (ParentId.HasValue)
                query = query.Where(a => a.ParentId == ParentId);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<AcademyCategory>(data, Skip, Take, queryCount);
        }
    }
    public class AcademyImageService : Repository<AcademyImage>, IAcademyImageService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public AcademyImageService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<AcademyImage> GetAllByFilters(int AcademyId,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<AcademyImage> query = context.Set<AcademyImage>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreAcademyImageDefaults.AllByFilters,
            AcademyId,
            Skip,
            Take);

            query = query.Where(a => a.AcademyId == AcademyId);

            int queryCount = query.Count();

            var data = query.OrderBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<AcademyImage>(data, Skip, Take, queryCount);
        }
    }
    public class AcademyFileService : Repository<AcademyFile>, IAcademyFileService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public AcademyFileService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<AcademyFile> GetAllByFilters(int AcademyId,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<AcademyFile> query = context.Set<AcademyFile>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreAcademyFileDefaults.AllByFilters,
            AcademyId,
            Skip,
            Take);

            query = query.Where(a => a.AcademyId == AcademyId);

            int queryCount = query.Count();

            var data = query.OrderBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<AcademyFile>(data, Skip, Take, queryCount);
        }
    }
    public class AcademyVideoService : Repository<AcademyVideo>, IAcademyVideoService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public AcademyVideoService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<AcademyVideo> GetAllByFilters(int AcademyId,
            AcademyVideoResource? AcademyVideoResource = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<AcademyVideo> query = context.Set<AcademyVideo>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreAcademyVideoDefaults.AllByFilters,
            AcademyVideoResource,
            AcademyId,
            Skip,
            Take);

            if (AcademyVideoResource.HasValue)
                query = query.Where(a => a.AcademyVideoResource == AcademyVideoResource);

            query = query.Where(a => a.AcademyId == AcademyId);

            int queryCount = query.Count();

            var data = query.OrderBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<AcademyVideo>(data, Skip, Take, queryCount);
        }
    }
}
