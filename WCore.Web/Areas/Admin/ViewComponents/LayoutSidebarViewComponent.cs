using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Roles;
using WCore.Framework;
using WCore.Services.Menus;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Roles;
using System.Collections.Generic;
using System.Linq;

namespace WCore.Web.Areas.Admin.ViewComponents
{
    public class LayoutSidebarViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IWorkContext _workContext;
        public LayoutSidebarViewComponent(IMenuService menuService, IWorkContext workContext)
        {
            this._menuService = menuService;
            this._workContext = workContext;
        }
        public virtual IViewComponentResult Invoke()
        {
            var menus = _menuService.GetAllSubUserMenusWithParent(_workContext.CurrentUser.RoleGroupId, null, AreaNames.Admin).Select(menu =>
              {
                  var m = menu.ToModel<MenuModel>();

                  var hasParent = _menuService.GetUserMenuByParentId(_workContext.CurrentUser.RoleGroupId, menu.Id, AreaNames.Admin);
                  if (hasParent.Any())
                      m.SubMenus = GetAllSubUserMenusWithParent(_workContext.CurrentUser.RoleGroupId, menu.Id);
                  return m;
              }).ToList();
            return View(menus);
        }

        public List<MenuModel> GetAllSubUserMenusWithParent(int roleGroupId, int? parentId = null)
        {
            var menus = _menuService.GetUserMenuByParentId(roleGroupId, parentId, AreaNames.Admin).Select(menu =>
             {
                 var m = menu.ToModel<MenuModel>();

                 var hasParent = _menuService.GetUserMenuByParentId(_workContext.CurrentUser.RoleGroupId, menu.Id, AreaNames.Admin);
                 if (hasParent.Any())
                     m.SubMenus = GetAllSubUserMenusWithParent(_workContext.CurrentUser.RoleGroupId, menu.Id);
                 return m;
             }).ToList();
            return menus;
        }
    }
}
