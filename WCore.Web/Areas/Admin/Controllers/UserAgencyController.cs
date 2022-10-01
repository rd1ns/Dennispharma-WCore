using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Core.Domain.Users;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Settings;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.UserAgencies;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class UserAgencyController : BaseAdminController
    {
        #region Fields
        private readonly IUserAgencyService _userAgencyService;

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor
        public UserAgencyController(IUserAgencyService userAgencyService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._userAgencyService = userAgencyService;
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
            return Redirect("/Admin/UserAgency/List");
        }

        public IActionResult List()
        {
            var model = new UserAgencySearchModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(UserAgencySearchModel searchModel)
        {
            var userAgencys = _userAgencyService.GetAllByFilters(searchModel.Query, searchModel.IsActive, searchModel.Deleted, searchModel.skip, searchModel.take);

            var model = new UserAgencyListModel().PrepareToGrid(searchModel, userAgencys, () =>
            {
                return userAgencys.Select(userAgency =>
                {
                    var m = userAgency.ToModel<UserAgencyModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var model = _userAgencyService.GetById(Id).ToModel<UserAgencyModel>();

            if (model == null)
                model = new UserAgencyModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(UserAgencyModel userAgency, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = userAgency.ToEntity<UserAgency>();

            if (delete)
            {
                var _entity = _userAgencyService.GetById(userAgency.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _userAgencyService.Update(_entity);
                return Json("Deleted");
            }


            if (userAgency.Id == 0)
            {
                _userAgencyService.Insert(entity);
            }
            else
            {
                _userAgencyService.Update(entity);
            }

            return Json(continueEditing);
        }
        #endregion
    }
}
