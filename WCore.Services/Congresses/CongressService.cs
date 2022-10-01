using System;
using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Congresses;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;

namespace WCore.Services.Congresses
{
    public class CongressService : Repository<Congress>, ICongressService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<Congress> GetAllByFilters(string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            DateTime? StartDate = null,
            DateTime? EndDate = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<Congress> query = context.Set<Congress>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressesDefaults.AllByFilters,
                Title,
                IsArchived,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take,
                StartDate,
                EndDate);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (StartDate.HasValue)
                query = query.Where(o => o.StartDate >= StartDate.Value);

            if (EndDate.HasValue)
                query = query.Where(discount => discount.EndDate <= EndDate.Value);

            if (IsArchived.HasValue)
                query = query.Where(a => a.IsArchived == IsArchived);

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.StartDate).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<Congress>(data, Skip, Take, queryCount);
        }
    }
    public class CongressPaperTypeService : Repository<CongressPaperType>, ICongressPaperTypeService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressPaperTypeService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<CongressPaperType> GetAllByFilters(int? CongressId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<CongressPaperType> query = context.Set<CongressPaperType>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressPaperTypesDefaults.AllByFilters,
                CongressId,
                Title,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take);

            if (CongressId.HasValue)
                query = query.Where(a => a.CongressId == CongressId);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<CongressPaperType>(data, Skip, Take, queryCount);
        }
    }
    public class CongressPaperService : Repository<CongressPaper>, ICongressPaperService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressPaperService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<CongressPaper> GetAllByFilters(int? CongressPaperTypeId = null,
            int? CongressId = null,
            string Title = "",
            string Code = "",
            string OwnersName = "",
            string OwnersSurname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<CongressPaper> query = context.Set<CongressPaper>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressPapersDefaults.AllByFilters,
            CongressId,
            CongressPaperTypeId,
            Title,
            Code,
            OwnersName,
            OwnersSurname,
            IsActive,
            Deleted,
            ShowOn,
            Skip,
            Take);

            if (CongressId.HasValue)
                query = query.Where(a => a.CongressId == CongressId);

            if (CongressPaperTypeId.HasValue)
                query = query.Where(a => a.CongressPaperTypeId == CongressPaperTypeId);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (!string.IsNullOrEmpty(Code))
                query = query.Where(a => a.Title.Contains(Code));

            if (!string.IsNullOrEmpty(OwnersName))
                query = query.Where(a => a.Title.Contains(OwnersName));

            if (!string.IsNullOrEmpty(OwnersSurname))
                query = query.Where(a => a.Title.Contains(OwnersSurname));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<CongressPaper>(data, Skip, Take, queryCount);
        }
    }
    public class CongressPresentationTypeService : Repository<CongressPresentationType>, ICongressPresentationTypeService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressPresentationTypeService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<CongressPresentationType> GetAllByFilters(int? CongressId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<CongressPresentationType> query = context.Set<CongressPresentationType>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressPresentationTypesDefaults.AllByFilters,
                CongressId,
                Title,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take);

            if (CongressId.HasValue)
                query = query.Where(a => a.CongressId == CongressId);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<CongressPresentationType>(data, Skip, Take, queryCount);
        }
    }
    public class CongressPresentationService : Repository<CongressPresentation>, ICongressPresentationService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressPresentationService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<CongressPresentation> GetAllByFilters(int? CongressPresentationTypeId = null,
            int? CongressId = null,
            string Title = "",
            string Code = "",
            string OwnersName = "",
            string OwnersSurname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<CongressPresentation> query = context.Set<CongressPresentation>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressPresentationsDefaults.AllByFilters,
            CongressId,
            CongressPresentationTypeId,
            Title,
            Code,
            OwnersName,
            OwnersSurname,
            IsActive,
            Deleted,
            ShowOn,
            Skip,
            Take);

            if (CongressId.HasValue)
                query = query.Where(a => a.CongressId == CongressId);

            if (CongressPresentationTypeId.HasValue)
                query = query.Where(a => a.CongressPresentationTypeId == CongressPresentationTypeId);

            if (!string.IsNullOrEmpty(Title))
                query = query.Where(a => a.Title.Contains(Title));

            if (!string.IsNullOrEmpty(Code))
                query = query.Where(a => a.Title.Contains(Code));

            if (!string.IsNullOrEmpty(OwnersName))
                query = query.Where(a => a.Title.Contains(OwnersName));

            if (!string.IsNullOrEmpty(OwnersSurname))
                query = query.Where(a => a.Title.Contains(OwnersSurname));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<CongressPresentation>(data, Skip, Take, queryCount);
        }
    }
    public class CongressImageService : Repository<CongressImage>, ICongressImageService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public CongressImageService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<CongressImage> GetAllByFilters(int CongressId,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<CongressImage> query = context.Set<CongressImage>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCongressImagesDefaults.AllByFilters,
            CongressId,
            Skip,
            Take);

            query = query.Where(a => a.CongressId == CongressId);

            int queryCount = query.Count();

            var data = query.OrderBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<CongressImage>(data, Skip, Take, queryCount);
        }
    }
}
