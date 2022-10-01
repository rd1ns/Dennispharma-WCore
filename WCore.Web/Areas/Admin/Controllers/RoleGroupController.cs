using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Roles;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Services.Menus;
using WCore.Services.Roles;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Roles;
using System;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class RoleGroupController : BaseAdminController
    {
        #region Fields
        private readonly IRoleGroupService _roleGroupService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor
        public RoleGroupController(IRoleGroupService roleGroupService,
            IRoleService roleService,
            IMenuService menuService,
            IUserService userService,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            this._roleGroupService = roleGroupService;
            this._roleService = roleService;
            this._menuService = menuService;
            this._userService = userService;
            this._workContext = workContext;
            this._webHelper = webHelper;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFilteredItems(int? userId = null)
        {
            var model = _roleGroupService.GetAllByFilters();
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _roleGroupService.GetById(Id).ToModel<RoleGroupModel>();

            if (entity == null)
                entity = new RoleGroupModel();

            entity.RoleGroupTypes = entity.RoleGroupType.ToSelectList().ToList();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(RoleGroupModel model, bool continueEditing, bool delete, bool sendInfo)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = model.ToEntity<RoleGroup>();

            if (model.Id == 0)
            {
                entity = _roleGroupService.Insert(entity);
            }
            _roleGroupService.Update(entity);

            return Json(continueEditing);
        }
        #endregion

        [HttpPost]
        public JsonResult GetMenuFilteredItems(int roleGroupId)
        {
            var model = _menuService.GetAll().OrderBy(o => o.DisplayOrder).Select(m =>
            {
                var menu = m.ToModel<MenuModel>();
                var roleGroups = _roleService.GetMenuIdRoleGroupId(roleGroupId, m.Id);
                menu.RoleMenu = roleGroups == null ? false : true;
                return menu;
            }).ToList();
            return Json(model);
        }

        [HttpPost]
        public IActionResult ChangeRoleMenu(int roleGroupId, int menuId)
        {
            var role = _roleService.GetMenuIdRoleGroupId(roleGroupId, menuId);
            if (role == null)
            {
                var entity = new RoleModel()
                {
                    MenuId = menuId,
                    RoleGroupId = roleGroupId
                }.ToEntity<Role>();
                _roleService.Insert(entity);
            }
            else
            {
                _roleService.Delete(role.Id);
            }
            return Json("OK");
        }
    }
}
