using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Core.Domain.Templates;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Services.Templates;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Templates;
using WCore.Web.Areas.Admin.Models.Users;
using System;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class TemplateController : BaseAdminController
    {
        #region Fields
        private readonly ITemplateService _templateService;

        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor
        public TemplateController(ITemplateService templateService,
            IUserService userService,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            this._templateService = templateService;
            this._userService = userService;
            this._workContext = workContext;
            this._webHelper = webHelper;

            CreatedUserId = _workContext.CurrentUser.Id;
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
        public JsonResult GetFilteredItems(int? userId = null)
        {
            var _skip = int.Parse(Request.Form["skip"]);
            var _take = int.Parse(Request.Form["take"]);
            var _query = Request.Form["filter[filters][0][value]"].ToString();

            var model = _templateService.GetAllByFilters((userId.HasValue ? userId : CreatedUserId), _query, skip: _skip, take: _take);
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id, int? companyId = null, bool IsPopup = false)
        {
            var entity = _templateService.GetById(Id).ToModel<TemplateModel>();

            if (entity == null)
                entity = new TemplateModel();

            entity.TemplateTypes = entity.TemplateType.ToSelectList().ToList();


            var users = _userService.GetAllByFilters(skip: 0, take: int.MaxValue);
            entity.Users = new System.Collections.Generic.List<SelectListItem>();
            //foreach (var item in users)
            //{
            //    entity.Users.Add(new SelectListItem() { Text = (item.FirstName + " " + item.LastName), Value = item.Id.ToString(), Selected = (entity.CreatedUserId == item.Id) });
            //}


            if (entity.Id == 0)
            {
                entity.CreatedUserId = _workContext.CurrentUser.Id;
                entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
            }
            else
            {
                if (entity.CreatedUserId == 0)
                {
                    entity.CreatedUserId = _workContext.CurrentUser.Id;
                    entity.CreatedUser = _workContext.CurrentUser.ToModel<UserModel>();
                }
                entity.CreatedUser = _userService.GetById(entity.CreatedUserId).ToModel<UserModel>();
            }

            entity.UserId = _workContext.CurrentUser.Id;
            entity.User = _workContext.CurrentUser.ToModel<UserModel>();


            return View(entity);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(TemplateModel model, bool continueEditing, bool delete)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = model.ToEntity<Template>();

            if (delete)
            {
                var _entity = _templateService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _templateService.Update(_entity);
                return Json("Deleted");
            }

            if (model.Id == 0)
            {
                _templateService.Insert(entity);
            }
            else
            {
                _templateService.Update(entity);
            }


            return Json(continueEditing);
        }
        #endregion
    }
}
