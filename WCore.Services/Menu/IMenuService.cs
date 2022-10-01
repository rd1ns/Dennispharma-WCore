using WCore.Core;
using WCore.Core.Domain.Roles;
using System.Collections.Generic;
using System.Linq;

namespace WCore.Services.Menus
{
    public interface IMenuService : IRepository<Menu>
    {
        IPagedList<Menu> GetAllByFilters(int? parentId = null, string searchValue = "", string AreaName = "Admin", int skip = 0, int take = 10);
        Menu GetMenuById(int? menuId = null, string ControllerName = "", string ActionName = "", string AreaName = "");
        IPagedList<Menu> GetMenuByParentId(int? parentId = null, bool? IsActive = null, string AreaName = "", int skip = 0, int take = 10);
        List<Menu> GetAllMenusWithParent(int? parentId = null);
        List<Menu> GetAllSubMenusWithParent(int? parentId = null);
        List<Menu> MenusWithBreadcrumb(List<Menu> menuList = null, int? parentId = null, string Title = "", bool IsSub = false, string AreaName = "", int skip = 0, int take = 10);
        List<Menu> GetUserMenuByParentId(int roleGroupId, int? parentId = null, string AreaName = "Admin");
        List<Menu> GetAllSubUserMenusWithParent(int roleGroupId, int? parentId = null, string AreaName = "Admin");
        IQueryable<Menu> GetUserMenu(int roleGroupId);
    }
}
