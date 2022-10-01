using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Roles;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.UserAgencies;
using WCore.Web.Areas.Admin.Models.Users;
using System.IO;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IUserAgencyService _userAgencyService;
        private readonly IRoleGroupService _roleGroupService;

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor
        public UserController(IUserService userService,
            IUserAgencyService userAgencyService,
            IRoleGroupService roleGroupService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._userService = userService;
            this._userAgencyService = userAgencyService;
            this._roleGroupService= roleGroupService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._workContext = workContext;

        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/User/List");
        }

        public IActionResult List()
        {
            var model = new UserSearchModel();

            model.UserType = UserType.Registered;

            model.UserTypes = model.UserType.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("Admin.Configuration.All")).ToList();
            model.UserAgencies = _userAgencyService.GetAllByFilters("", true, false, 0, int.MaxValue).Select(o =>
            {
                var s = new SelectListItem();
                s.Text = o.Name;
                s.Value = o.Id.ToString();
                s.Selected = o.Id == model.UserAgencyId ? true : false;
                return s;
            }).InsertEmptyFirst(_localizationService.GetResource("Admin.Configuration.All")).ToList();
            model.RoleGroups = _roleGroupService.GetAllByFilters("",null,0,int.MaxValue).Select(o =>
            {
                var s = new SelectListItem();
                s.Text = o.Name;
                s.Value = o.Id.ToString();
                s.Selected = o.Id == model.UserAgencyId ? true : false;
                return s;
            }).InsertEmptyFirst(_localizationService.GetResource("Admin.Configuration.All")).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(UserSearchModel searchModel)
        {
            var users = _userService.GetAllByFilters(searchValue: searchModel.Query, searchModel.UserType, searchModel.UserAgencyId, searchModel.IsAgencyManager, searchModel.IsAgencyStaff, searchModel.IsActive, searchModel.Deleted, skip: searchModel.skip, take: searchModel.take);

            var model = new UserListModel().PrepareToGrid(searchModel, users, () =>
            {
                return users.Select(user =>
                {
                    var m = user.ToModel<UserModel>();
                    m.UserTypeName = user.UserType.GetLocalizedEnum(_localizationService, _workContext);
                    //m.UserAgency = _userAgencyService.GetById(user.UserAgencyId).ToModel<UserAgencyModel>();
                    //m.RoleGroup = _roleGroupService.GetById(user.RoleGroupId).ToModel<WCore.Web.Areas.Admin.Models.Roles.RoleGroupModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id,int? userAgencyId = null)
        {
            var model = _userService.GetUserById(Id).ToModel<UserModel>();

            if (model == null)
                model = new UserModel();

            model.UserTypes = model.UserType.ToSelectList().ToList();

            if (userAgencyId.HasValue)
                model.UserAgencyId = userAgencyId.Value;

            model.UserAgencies = _userAgencyService.GetAllByFilters("", true, false, 0, int.MaxValue).Select(o =>
            {
                var s = new SelectListItem();
                s.Text = o.Name;
                s.Value = o.Id.ToString();
                s.Selected = o.Id == model.UserAgencyId ? true : false;
                return s;
            }).InsertEmptyFirst(_localizationService.GetResource("Admin.Configuration.Agency") + " " + _localizationService.GetResource("Admin.Configuration.NotSelected")).ToList();

            model.RoleGroups = _roleGroupService.GetAllByFilters("", null, 0, int.MaxValue).Select(o =>
            {
                var s = new SelectListItem();
                s.Text = o.Name;
                s.Value = o.Id.ToString();
                s.Selected = o.Id == model.UserAgencyId ? true : false;
                return s;
            }).InsertEmptyFirst(_localizationService.GetResource("Admin.Configuration.NotSelected")).ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(UserModel user, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = user.ToEntity<User>();

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    file.CopyToAsync(fileStream);
                    //entity.Avatar = "/uploads/" + file.FileName;
                }
            }

            if (!Request.Form.Files.Any() && user.Id != 0)
            {
                var u = _userService.GetById(user.Id);
                //entity.Avatar = u.Avatar;
            }

            if (delete)
            {
                var _entity = _userService.GetById(user.Id);
                _entity.Deleted = true;
                //_entity.IsActive = false;
                _userService.Update(_entity);
                return Json("Deleted");
            }


            if (user.Id == 0)
            {
                _userService.Insert(entity);
            }
            else
            {
                _userService.Update(entity);
            }



            if (sendInfo)
            {
                //var sendRegisterNotification = _settingService.GetSetting().NotificationSetting.SendRegisterNotification;
                //if (sendRegisterNotification && !string.IsNullOrEmpty(entity.EMail))
                //{
                //    var html = "WCore Portal kullanıcı kaydınız tamamlanmıştır. </br>";
                //    html += "<b> Bilgiler </b></br>";
                //    html += "Kullanıcı Adı : " + entity.EMail + " </br>";
                //    html += "Şifre : " + entity.Password + " </br>";
                //    _webHelper.MailSender(html, "Kullanıcı hesabınız oluşturulmuştur.", entity.EMail);
                //}
            }

            return Json(continueEditing);
        }
        #endregion
    }
}