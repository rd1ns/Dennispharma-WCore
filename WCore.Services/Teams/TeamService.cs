using System.Data.Entity;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Teams;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;

namespace WCore.Services.Teams
{
    public class TeamService : Repository<Team>, ITeamService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public TeamService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<Team> GetAllByFilters(int? TeamCategoryId = null,
            string Name = "",
            string Surname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? ShowOnHome = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<Team> query = context.Set<Team>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreTeamsDefaults.AllByFilters,
                TeamCategoryId,
                Name,
                Surname,
                IsActive,
                Deleted,
                ShowOn,
                ShowOnHome,
                Skip,
                Take);

            if (TeamCategoryId.HasValue)
                query = query.Where(a => a.TeamCategoryId == TeamCategoryId.Value);

            if (!string.IsNullOrEmpty(Name))
                query = query.Where(a => a.Name.Contains(Name));

            if (!string.IsNullOrEmpty(Surname))
                query = query.Where(a => a.Surname.Contains(Surname));

            if (IsActive.HasValue)
                query = query.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                query = query.Where(a => a.Deleted == Deleted);

            if (ShowOn.HasValue)
                query = query.Where(a => a.ShowOn == ShowOn);

            int queryCount = query.Count();

            var data = query.OrderByDescending(o => o.IsActive).Skip(Skip).Take(Take).ToCachedList(cacheKey);

            return new PagedList<Team>(data, Skip, Take, queryCount);
        }
    }
    public class TeamCategoryService : Repository<TeamCategory>, ITeamCategoryService
    {
        private readonly ICacheKeyService _cacheKeyService;
        public TeamCategoryService(WCoreContext context,
            ICacheKeyService cacheKeyService) : base(context)
        {
            this._cacheKeyService = cacheKeyService;
        }

        public IPagedList<TeamCategory> GetAllByFilters(int? ParentId = 0,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue)
        {
            IQueryable<TeamCategory> query = context.Set<TeamCategory>();

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreTeamCategoriesDefaults.AllByFilters,
                ParentId,
                Title,
                IsActive,
                Deleted,
                ShowOn,
                Skip,
                Take);

            if (ParentId.HasValue)
                query = query.Where(a => a.ParentId == ParentId);

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

            return new PagedList<TeamCategory>(data, Skip, Take, queryCount);
        }
    }
}
