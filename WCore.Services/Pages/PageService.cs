using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Pages;
using WCore.Services.Users;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using WCore.Services.Caching;
using WCore.Services.Common;

namespace WCore.Services.Pages
{
    public class PageService : Repository<Page>, IPageService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public PageService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<Page> GetAllByFilters(string searchValue = "",
            PageType? PageType = null,
            FooterLocation? FooterLocation = null,
            int? ParentId = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? HomePage = null,
            bool? RedirectPage = null,
            int skip = 0,
            int take = int.MaxValue)
        {
            IQueryable<Page> recordsFiltered = context.Set<Page>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCorePagesDefaults.AllByFilters, searchValue, PageType, FooterLocation, ParentId, IsActive, Deleted, ShowOn, HomePage, RedirectPage, skip, take);

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(a => a.Title.Contains(searchValue));

            if (FooterLocation.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.FooterLocation == FooterLocation);

            if (ParentId.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.ParentId == ParentId);

            if (PageType.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.PageType == PageType);

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.ShowOn == ShowOn);

            if (HomePage.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.HomePage == HomePage);

            if (RedirectPage.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.RedirectPage == RedirectPage);

            var result = _staticCacheManager.Get(key, () =>
            {
                int recordsFilteredCount = recordsFiltered.Count();

                int recordsTotalCount = context.Set<Page>().Count();

                var data = recordsFiltered.OrderBy(o => o.DisplayOrder).Skip(skip).Take(take).ToList();

                return new PagedList<Page>(data, skip, take, recordsFilteredCount);
            });
            return result;

        }

        public List<Page> PagesWithBreadcrumb(List<Page> _pageList = null,
            int? ParentId = null,
            bool? ShowOn = null,
            bool? IsSub = false,
            string Title = "")
        {
            if (_pageList == null && IsSub == false)
            {
                _pageList = new List<Page>();
            }

            var pages = GetAllByFilters(ParentId: ParentId).ToList().OrderBy(o => o.DisplayOrder);

            foreach (var page in pages)
            {
                page.Title = Title + page.Title;
                _pageList.Add(page);
                var subContent = GetAllByFilters(ParentId: page.Id, ShowOn: ShowOn).ToList();
                if (subContent.Count > 0)
                {
                    PagesWithBreadcrumb(_pageList, ParentId: page.Id, Title: Title + page.Title + " > ", IsSub: true);
                }
            }

            return _pageList;
        }

        public Page GetHomePage()
        {
            IQueryable<Page> recordsFiltered = context.Set<Page>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCorePagesDefaults.HomePage);

            var result = _staticCacheManager.Get(key, () =>
            {
                return recordsFiltered.FirstOrDefault(o => o.HomePage);
            });
            return result;
        }
    }
}
