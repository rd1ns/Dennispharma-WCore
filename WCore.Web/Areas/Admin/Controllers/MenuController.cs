using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Roles;
using WCore.Framework.Filters;
using WCore.Services.Menus;
using WCore.Services.Roles;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Roles;
using System;
using System.Linq;
using WCore.Services.Localization;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class MenuController : BaseAdminController
    {
        #region Fields
        private readonly IMenuService _menuService;
        private readonly IRoleService _roleService;
        private readonly ILocalizationService _localizationService;

        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor
        public MenuController(IMenuService menuService,
            IRoleService roleService,
            IUserService userService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            this._menuService = menuService;
            this._roleService = roleService;
            this._localizationService = localizationService;
            this._userService = userService;
            this._workContext = workContext;
            this._webHelper = webHelper;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return RedirectToAction("list");
        }

        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFilteredItems()
        {
            var model = _menuService.GetAllByFilters(skip: 0, take: int.MaxValue);
            foreach (var item in model)
            {
                item.Name = _localizationService.GetResource(item.Name,_workContext.WorkingLanguage.Id);
            }
            return Json(model.OrderBy(o => o.DisplayOrder).ThenBy(o => !o.IsHidden));
        }

        public IActionResult AddOrEdit(int Id, int ParentId)
        {
            var entity = _menuService.GetById(Id).ToModel<MenuModel>();

            if (entity == null)
                entity = new MenuModel();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(MenuModel model, bool continueEditing, bool delete)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = model.ToEntity<Menu>();

            if (model.Id == 0)
                entity = _menuService.Insert(entity);

            _menuService.Update(entity);

            return Json(continueEditing);
        }
        #endregion

        [HttpPost]
        public IActionResult ChangeIsHiddenMenu(int menuId, bool isChecked)
        {
            var menu = _menuService.GetById(menuId);
            menu.IsHidden = isChecked;
            _menuService.Update(menu);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult ChangeIsActiveMenu(int menuId, bool isChecked)
        {
            var menu = _menuService.GetById(menuId);
            menu.IsActive = isChecked;
            _menuService.Update(menu);
            return Json("OK");
        }
    }
}
