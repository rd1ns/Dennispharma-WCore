using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Common;
using System.Linq;
using WCore.Services.Caching;
using WCore.Services.Directory;
using WCore.Services.Caching.Extensions;

namespace WCore.Services.Common
{
    public class CityService : Repository<City>, ICityService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public CityService(WCoreContext context,
            IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService = cacheKeyService;
        }

        public virtual IPagedList<City> GetAllByFilters(int CountryId,
            string Name = "",
            bool? IsActive = null,
            bool? Deleted = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<City> query = context.Set<City>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCityDefaults.AllByFilters,
                CountryId,
                Name,
                IsActive,
                Deleted,
                Skip,
                Take);

            query = query.Where(a => a.CountryId == CountryId);

            if (!string.IsNullOrEmpty(Name))
                query = query.Where(a => a.Name.Contains(Name));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<City>(data, Skip, Take, queryCount);
        }
    }
}