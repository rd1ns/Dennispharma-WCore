using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Popup;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Common;

namespace WCore.Services.Popups
{
    public class PopupService : Repository<Popup>, IPopupService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public PopupService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<Popup> GetAllByFilters(string ShowUrl = "",
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<Popup> query = context.Set<Popup>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCorePopupsDefaults.AllByFilters,
                ShowUrl,
                ShowOn,
                Skip,
                Take);

            if (!string.IsNullOrEmpty(ShowUrl))
                query = query.Where(a => a.ShowUrl.Contains(ShowUrl));

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<Popup>(data, Skip, Take, queryCount);
        }
    }
}
