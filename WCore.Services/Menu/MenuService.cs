using Microsoft.EntityFrameworkCore;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Roles;
using System.Collections.Generic;
using System.Linq;
using WCore.Services.Caching;

namespace WCore.Services.Menus
{
    public class MenuService : Repository<Menu>, IMenuService
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheKeyService _cacheKeyService;
        public MenuService(IStaticCacheManager staticCacheManager,
            ICacheKeyService cacheKeyService,
            WCoreContext context) : base(context)
        {
            this._staticCacheManager = staticCacheManager;
            this._cacheKeyService= cacheKeyService;
        }

        public IPagedList<Menu> GetAllByFilters(int? parentId = null, string searchValue = "", string AreaName = "Admin", int skip = 0, int take = 10)
        {
            IQueryable<Menu> recordsFiltered = context.Set<Menu>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreMenuDefaults.GetAllByFiltersCacheKey, parentId, searchValue, AreaName, skip, take);

            if (parentId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.ParentId == parentId.Value);

            if (!string.IsNullOrEmpty(AreaName))
                recordsFiltered = recordsFiltered.Where(o => o.AreaName == AreaName);

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(a => a.Name.Contains(searchValue));


            var menus = _staticCacheManager.Get(key, () =>
            {

                int recordsFilteredCount = recordsFiltered.Count();

                int recordsTotalCount = context.Set<Menu>().Count();

                var data = recordsFiltered.OrderByDescending(o => o.IsActive).OrderByDescending(o => o.DisplayOrder).Skip(skip).Take(take).ToList();

                return new PagedList<Menu>(data, skip, take, recordsFilteredCount);
            });
            return menus;
        }
        public virtual Menu GetMenuById(int? menuId = null, string ControllerName = "", string ActionName = "", string AreaName = "")
        {
            IQueryable<Menu> recordsFiltered = context.Set<Menu>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreMenuDefaults.GetMenuByIdCacheKey, menuId, ControllerName, ActionName, AreaName);

            if (menuId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.Id == menuId);

            if (!string.IsNullOrEmpty(ControllerName))
                recordsFiltered = recordsFiltered.Where(o => o.Controller == ControllerName);

            if (!string.IsNullOrEmpty(ActionName))
                recordsFiltered = recordsFiltered.Where(o => o.Action == ActionName);

            if (!string.IsNullOrEmpty(AreaName))
                recordsFiltered = recordsFiltered.Where(o => o.AreaName == AreaName);

            var entity = _staticCacheManager.Get(key, recordsFiltered.FirstOrDefault);

            return entity;
        }
        public virtual IPagedList<Menu> GetMenuByParentId(int? parentId = null, bool? IsActive = null, string AreaName = "", int skip = 0, int take = 10)
        {


            IQueryable<Menu> recordsFiltered = context.Set<Menu>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreMenuDefaults.GetMenuByParentIdCacheKey, parentId, IsActive, AreaName, skip, take);

            if (parentId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.ParentId == parentId.Value);

            if (!string.IsNullOrEmpty(AreaName))
                recordsFiltered = recordsFiltered.Where(o => o.AreaName == AreaName);

            recordsFiltered = recordsFiltered.OrderByDescending(o => o.IsActive).OrderByDescending(o => o.DisplayOrder).Skip(skip).Take(take);

            var menus = _staticCacheManager.Get(key, () =>
            {

                int recordsFilteredCount = recordsFiltered.Count();

                int recordsTotalCount = context.Set<Menu>().Count();

                var data = recordsFiltered.ToList();

                return new PagedList<Menu>(data, skip, take, recordsFilteredCount);
            });
            return menus;
        }
        public virtual List<Menu> GetAllMenusWithParent(int? parentId = null)
        {
            var menuList = GetMenuByParentId(parentId).ToList();

            //foreach (var menu in menuList)
            //{
            //    var hasParent = GetMenuByParentId(menu.Id);
            //    if (hasParent.Any())
            //        menu.SubMenus = GetAllSubMenusWithParent(parentId: menu.Id);
            //}
            return menuList;
        }
        public virtual List<Menu> GetAllSubMenusWithParent(int? parentId = null)
        {
            var menuList = GetMenuByParentId(parentId, take: int.MaxValue).ToList();

            //foreach (var menu in menuList)
            //{
            //    var hasParent = GetMenuByParentId(menu.Id);
            //    if (hasParent.Any())
            //        menu.SubMenus = GetAllSubMenusWithParent(parentId: menu.Id);
            //}
            return menuList;
        }
        public List<Menu> MenusWithBreadcrumb(List<Menu> menuList = null, int? parentId = null, string Title = "", bool IsSub = false, string AreaName = "", int skip = 0, int take = 10)
        {
            if (menuList == null && IsSub == false)
            {
                menuList = new List<Menu>();
            }
            var menus = GetAllByFilters(parentId, "", AreaName, skip, take);

            foreach (var menu in menus)
            {
                var p = menu;
                p.Name = Title + menu.Name;
                menuList.Add(p);

                var subContent = GetAllByFilters(parentId, "", AreaName, skip, take);
                if (subContent.Count > 0)
                {
                    MenusWithBreadcrumb(menuList, menu.Id, Title + menu.Name + " > ", true, AreaName);
                }
            }
            return menuList;
        }
        public virtual List<Menu> GetUserMenuByParentId(int roleGroupId, int? parentId = null, string AreaName = "Admin")
        {
            IQueryable<Menu> recordsFiltered = GetUserMenu(roleGroupId);

            recordsFiltered = recordsFiltered.Where(o => o.ParentId == parentId);

            if (!string.IsNullOrEmpty(AreaName))
                recordsFiltered = recordsFiltered.Where(o => o.AreaName == AreaName);

            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreMenuDefaults.GetUserMenuByParentIdCacheKey, roleGroupId, parentId, AreaName);

            var menus = _staticCacheManager.Get(cacheKey, recordsFiltered.OrderBy(o => o.DisplayOrder).ToList);

            return menus;
        }
        public virtual List<Menu> GetAllSubUserMenusWithParent(int roleGroupId, int? parentId = null, string AreaName = "Admin")
        {
            var menuList = GetUserMenuByParentId(roleGroupId, parentId, AreaName);
            return menuList;
        }
        public IQueryable<Menu> GetUserMenu(int roleGroupId)
        {
            var query = context.Menus.FromSqlRaw("Select M.* From Menus M " +
                                                 "Inner Join Roles R " +
                                                 "On M.Id=R.MenuId Where R.RoleGroupId=" + roleGroupId);
            return query;
        }
    }
    public static partial class WCoreMenuDefaults
    {
        #region Caching defaults
        public static CacheKey GetUserMenuByParentIdCacheKey => new CacheKey("WCore.menu.userroleandparent.{0}-{1}-{2}", "WCore.menu.userroleandparent.");
        public static CacheKey GetMenuByParentIdCacheKey => new CacheKey("WCore.menu.byparentid.{0}-{1}-{2}-{3}-{4}", "WCore.menu.byparentid.");
        public static CacheKey GetAllByFiltersCacheKey => new CacheKey("WCore.menu.byfilters.{0}-{1}-{2}-{3}-{4}", "WCore.menu.byfilters.");
        public static CacheKey GetMenuByIdCacheKey => new CacheKey("WCore.menu.byid.{0}-{1}-{2}-{3}", "WCore.menu.byid.");

        #endregion
    }
}
