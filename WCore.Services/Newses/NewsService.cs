using System;
using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Newses;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;

namespace WCore.Services.Newses
{
    public class NewsService : Repository<News>, INewsService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public NewsService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<News> GetAllByFilters(int? NewsCategoryId = null,
            string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? ShowOnHome = null,
            DateTime? StartDate = null,
            DateTime? EndDate = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<News> query = context.Set<News>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreNewsesDefaults.AllByFilters,
                NewsCategoryId,
                Title,
                IsArchived,
                IsActive,
                Deleted,
                ShowOn,
                ShowOnHome,
                Skip,
                Take,
                StartDate,
                EndDate);

            if (NewsCategoryId.HasValue)
                query = query.Where(a => a.NewsCategoryId == NewsCategoryId.Value);

            if (StartDate.HasValue)
                query = query.Where(o => o.StartDate >= StartDate.Value);

            if (EndDate.HasValue)
                query = query.Where(o => o.EndDate <= EndDate.Value);

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

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.StartDate).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<News>(data, Skip, Take, queryCount);
        }
    }
    public class NewsCategoryService : Repository<NewsCategory>, INewsCategoryService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public NewsCategoryService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<NewsCategory> GetAllByFilters(string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<NewsCategory> query = context.Set<NewsCategory>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreNewsCategoriesDefaults.AllByFilters,
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

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).ThenBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<NewsCategory>(data, Skip, Take, queryCount);
        }
    }
    public class NewsImageService : Repository<NewsImage>, INewsImageService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public NewsImageService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<NewsImage> GetAllByFilters(int NewsId, 
            int Skip = 0, 
            int Take = int.MaxValue)
        {
            IQueryable<NewsImage> query = context.Set<NewsImage>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreNewsImagesDefaults.AllByFilters,
            NewsId,
            Skip,
            Take);

            query = query.Where(a => a.NewsId == NewsId);

            int queryCount = query.Count();

            var data = query.OrderBy(o => o.DisplayOrder).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<NewsImage>(data, Skip, Take, queryCount);
        }
    }
}
