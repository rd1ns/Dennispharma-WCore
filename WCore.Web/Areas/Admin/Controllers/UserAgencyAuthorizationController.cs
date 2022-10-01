using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Framework.Extensions;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.UserAgencies;
using WCore.Web.Areas.Admin.Models.Users;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class UserAgencyAuthorizationController : BaseAdminController
    {
        #region Fields
        private readonly IUserAgencyAuthorizationService _userAgencyAuthorizationService;
        private readonly IUserService _userService;
        private readonly IUserAgencyService _userAgencyService;

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor
        public UserAgencyAuthorizationController(IUserAgencyAuthorizationService userAgencyAuthorizationService,
            IUserService userService,
            IUserAgencyService userAgencyService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._userAgencyAuthorizationService = userAgencyAuthorizationService;
            this._userService = userService;
            this._userAgencyService= userAgencyService;

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
            return Redirect("/Admin/UserAgencyAuthorization/List");
        }

        public IActionResult List()
        {
            var model = new UserAgencyAuthorizationSearchModel();
            model.UserAgencies = new SelectList(_userAgencyService.GetAllByFilters(), "Id", "Name").InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(UserAgencyAuthorizationSearchModel searchModel)
        {
            var userAgencyAuthorizations = _userAgencyAuthorizationService.GetAllByFilters(searchModel.UserAgencyId, searchModel.IsRead, searchModel.IsCreate, searchModel.IsUpdate, searchModel.IsDelete);

            var model = new UserAgencyAuthorizationListModel().PrepareToGrid(searchModel, userAgencyAuthorizations, () =>
            {
                return userAgencyAuthorizations.Select(userAgencyAuthorization =>
                {
                    var m = userAgencyAuthorization.ToModel<UserAgencyAuthorizationModel>();
                    m.UserAgency = _userAgencyService.GetById(userAgencyAuthorization.UserAgencyId).ToModel<UserAgencyModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id, int? userAgencyId = null)
        {
            var model = _userAgencyAuthorizationService.GetById(Id).ToModel<UserAgencyAuthorizationModel>();

            if (model == null)
                model = new UserAgencyAuthorizationModel();

            model.UserAgencies = new SelectList(_userAgencyService.GetAllByFilters(), "Id", "Name", model.UserAgencyId).ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(UserAgencyAuthorizationModel userAgencyAuthorization, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = userAgencyAuthorization.ToEntity<UserAgencyAuthorization>();

            if (delete)
            {
                _userAgencyAuthorizationService.Delete(userAgencyAuthorization.Id);
                return Json("Deleted");
            }

            if (userAgencyAuthorization.Id == 0)
            {
                _userAgencyAuthorizationService.Insert(entity);
            }
            else
            {
                _userAgencyAuthorizationService.Update(entity);
            }

            return Json(continueEditing);
        }
        #endregion
    }
}
